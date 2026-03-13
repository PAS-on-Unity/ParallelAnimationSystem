using MsdfgenNet;
using MsdfgenNet.Data;
using SharpFont;
using TmpIO;

namespace TmpGenerator;

public static class App
{
    public static void Run(Options options)
    {
        using var freetype = new Library();
        using var face = freetype.NewFace(options.Input, 0);
        face.SetPixelSizes(0, (uint)options.Size);
        
        var characterToGlyphId = options.Characters
            .ToDictionary(x => x, x => (int)face.GetCharIndex(x));
        var glyphIdToGlyph = characterToGlyphId.Values
            .Select(x => GetGlyph(x, face, options.Padding, options.Range))
            .ToDictionary(x => x.Id);
        var atlas = AtlasBuilder.Build(
            glyphIdToGlyph.Values
                .Where(x => x.Bitmap is not null)
                .Select(x => (x.Id, x.Bitmap!)),
            options.Gap);
        
        var tmpMetadata = new TmpMetadata(
            face.FamilyName, 
            options.Size, 
            face.Size.Metrics.Height.Value / 64.0f, 
            face.Size.Metrics.Ascender.Value / 64.0f,
            face.Size.Metrics.Descender.Value / 64.0f,
            (float) options.Range);
        var tmpCharacters = characterToGlyphId
            .Select(kvp => new TmpCharacter(kvp.Key, kvp.Value))
            .ToArray();
        var glyphIdToAtlasElement = atlas.Elements.ToDictionary(x => x.Id);
        var tmpGlyphs = glyphIdToGlyph.Values
            .Select(glyph =>
            {
                if (glyphIdToAtlasElement.TryGetValue(glyph.Id, out var element))
                {
                    var minX = element.X / (float) atlas.Width;
                    var minY = element.Y / (float) atlas.Height;
                    var maxX = (element.X + element.Width) / (float) atlas.Width;
                    var maxY = (element.Y + element.Height) / (float) atlas.Height;
                    return new TmpGlyph(
                        glyph.Id,
                        glyph.Advance,
                        glyph.BearingX, glyph.BearingY,
                        glyph.Width, glyph.Height,
                        minX, minY,
                        maxX, maxY);
                }
                
                return new TmpGlyph(
                    glyph.Id, 
                    glyph.Advance, 
                    glyph.BearingX, glyph.BearingY, 
                    glyph.Width, glyph.Height, 
                    0.0f, 0.0f, 0.0f, 0.0f);
            })
            .ToArray();
        var tmpAtlas = new TmpAtlas(atlas.Width, atlas.Height, atlas.Data.ToArray());
        var tmpFile = new TmpFile(tmpMetadata, tmpCharacters, tmpGlyphs, tmpAtlas);
        
        using var stream = File.Open(options.Output, FileMode.Create);
        TmpWrite.Write(stream, tmpFile);
    }

    private static Glyph GetGlyph(int glyphId, Face face, int padding, double range)
    {
        // Load glyph
        face.LoadGlyph((uint) glyphId, LoadFlags.Default, LoadTarget.Normal);
        var glyph = face.Glyph;
        var metrics = glyph.Metrics;

        if (metrics.Width.Value == 0 || metrics.Height.Value == 0)
            return new Glyph(glyphId, metrics.HorizontalAdvance.Value >> 6, 0, 0, 0, 0, null);
        
        // Build character shape from outline
        var outline = glyph.Outline;
        using var shape = ShapeBuilder.Build(outline);
        shape.InverseYAxis = true;
        shape.Normalize();
        
        // Fill in edge colors
        EdgeColoring.ColorByDistance(shape, 3.0);
        
        // Initialize bitmap
        var width = (metrics.Width.Value >> 6) + (padding << 1);
        var height = (metrics.Height.Value >> 6) + (padding << 1);
        var msdf = new Bitmap(width, height, 3);
        
        // Calculate glyph offset
        var offsetX = -metrics.HorizontalBearingX.Value / 64.0 + padding;
        var offsetY = -(metrics.HorizontalBearingY.Value - metrics.Height.Value) / 64.0 + padding;
        
        // Generate MSDF
        using var projection = new Projection(new MsdfVector2(1.0, 1.0), new MsdfVector2(offsetX, offsetY));
        using var transformation = new SdfTransformation(projection, DistanceMapping.CreateRange(new MsdfgenNet.Data.Range(-range * 0.5, range * 0.5)));
        using var config = new MsdfGeneratorConfig(true, default);
        Msdfgen.GenerateMsdf(msdf, shape, transformation, config);

        return new Glyph(
            glyphId,
            metrics.HorizontalAdvance.Value >> 6,
            metrics.HorizontalBearingX.Value / 64.0f - padding,
            metrics.HorizontalBearingY.Value / 64.0f + padding,
            metrics.Width.Value / 64.0f + padding * 2.0f,
            metrics.Height.Value / 64.0f + padding * 2.0f,
            msdf);
    }
}