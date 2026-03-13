using System.Runtime.InteropServices;
using SimpleStructuredBinaryFormat;
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable PossibleNullReferenceException

#nullable disable

namespace TmpIO;

public static class TmpRead
{
    public static TmpFile Read(Stream stream)
    {
        var obj = SsbfRead.ReadFromStream(stream);
        return new TmpFile(
            GetMetadata((SsbfObject)obj["metadata"]),
            GetCharacters((SsbfArray)obj["characters"]),
            GetGlyphs((SsbfArray)obj["glyphs"]),
            GetAtlas((SsbfObject)obj["atlas"])
        );
    }

    private static TmpMetadata GetMetadata(SsbfObject obj)
        => new(
            obj["name"].Get<string>(),
            obj["size"].Get<int>(),
            obj["lineHeight"].Get<float>(),
            obj["ascender"].Get<float>(),
            obj["descender"].Get<float>(),
            obj["sdfRange"].Get<float>()
        );

    private static TmpCharacter[] GetCharacters(SsbfArray ssbfArray)
    {
        var characters = new TmpCharacter[ssbfArray.Count];
        for (var i = 0; i < ssbfArray.Count; i++)
        {
            var obj = (SsbfObject)ssbfArray[i];
            characters[i] = new TmpCharacter(
                (char)obj["character"].Get<ushort>(),
                obj["glyphId"].Get<int>()
            );
        }
        return characters;
    }
    
    private static TmpGlyph[] GetGlyphs(SsbfArray ssbfArray)
    {
        var glyphs = new TmpGlyph[ssbfArray.Count];
        for (var i = 0; i < ssbfArray.Count; i++)
        {
            var obj = (SsbfObject)ssbfArray[i];
            glyphs[i] = new TmpGlyph(
                obj["id"].Get<int>(),
                obj["advance"].Get<float>(),
                obj["bearingX"].Get<float>(),
                obj["bearingY"].Get<float>(),
                obj["width"].Get<float>(),
                obj["height"].Get<float>(),
                obj["minX"].Get<float>(),
                obj["minY"].Get<float>(),
                obj["maxX"].Get<float>(),
                obj["maxY"].Get<float>()
            );
        }
        return glyphs;
    }
    
    private static TmpAtlas GetAtlas(SsbfObject ssbfObject)
    {
        var data = ssbfObject["data"].Get<byte[]>();
        return new TmpAtlas(
            ssbfObject["width"].Get<int>(),
            ssbfObject["height"].Get<int>(),
            MemoryMarshal.Cast<byte, TmpAtlasPixel>(data).ToArray()
        );
    }
}