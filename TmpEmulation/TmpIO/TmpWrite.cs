using System.Runtime.InteropServices;
using SimpleStructuredBinaryFormat;

namespace TmpIO;

public static class TmpWrite
{
    public static void Write(Stream stream, TmpFile file)
    {
        var obj = new SsbfObject
        {
            ["metadata"] = GetMetadata(file.Metadata),
            ["characters"] = GetCharacters(file.Characters),
            ["glyphs"] = GetGlyphs(file.Glyphs),
            ["atlas"] = GetAtlas(file.Atlas),
        };
        
        SsbfWrite.WriteToStream(stream, obj, Compression.Gzip);
    }
    
    private static SsbfObject GetMetadata(TmpMetadata metadata)
        => new()
        {
            ["name"] = metadata.Name,
            ["size"] = metadata.Size,
            ["lineHeight"] = metadata.LineHeight,
            ["ascender"] = metadata.Ascender,
            ["descender"] = metadata.Descender,
            ["sdfRange"] = metadata.SdfRange,
        };

    private static SsbfArray GetCharacters(TmpCharacter[] characters)
    {
        var array = new SsbfArray(characters.Length);
        foreach (var character in characters)
        {
            array.Add(new SsbfObject
            {
                ["character"] = (ushort) character.Character,
                ["glyphId"] = character.GlyphId,
            });
        }
        return array;
    }
    
    private static SsbfArray GetGlyphs(TmpGlyph[] glyphs)
    {
        var array = new SsbfArray(glyphs.Length);
        foreach (var glyph in glyphs)
        {
            array.Add(new SsbfObject
            {
                ["id"] = glyph.Id,
                ["advance"] = glyph.Advance,
                ["bearingX"] = glyph.BearingX,
                ["bearingY"] = glyph.BearingY,
                ["width"] = glyph.Width,
                ["height"] = glyph.Height,
                ["minX"] = glyph.MinX,
                ["minY"] = glyph.MinY,
                ["maxX"] = glyph.MaxX,
                ["maxY"] = glyph.MaxY,
            });
        }
        return array;
    }
    
    private static SsbfObject GetAtlas(TmpAtlas atlas)
        => new()
        {
            ["width"] = atlas.Width,
            ["height"] = atlas.Height,
            ["data"] = new SsbfByteArray(MemoryMarshal.Cast<TmpAtlasPixel, byte>(atlas.Data)),
        };
}