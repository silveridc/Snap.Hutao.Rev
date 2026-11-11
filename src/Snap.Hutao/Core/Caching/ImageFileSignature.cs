// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using System.IO;

namespace Snap.Hutao.Rev.Core.Caching;

// 静态资源站对缺失文件会以 200 + HTML 回落，必须用文件头挡住非图片内容进缓存
internal static class ImageFileSignature
{
    public static bool IsValidFile(string path)
    {
        using (FileStream stream = File.OpenRead(path))
        {
            byte[] header = new byte[12];
            int read = 0;
            while (read < header.Length)
            {
                int current = stream.Read(header, read, header.Length - read);
                if (current is 0)
                {
                    break;
                }

                read += current;
            }

            return IsValid(header.AsSpan(0, read));
        }
    }

    public static bool IsValid(ReadOnlySpan<byte> header)
    {
        ReadOnlySpan<byte> png = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];
        ReadOnlySpan<byte> jpeg = [0xFF, 0xD8, 0xFF];
        ReadOnlySpan<byte> ico = [0x00, 0x00, 0x01, 0x00];
        ReadOnlySpan<byte> bmp = "BM"u8;
        ReadOnlySpan<byte> riff = "RIFF"u8;
        ReadOnlySpan<byte> webp = "WEBP"u8;

        return header.StartsWith(png)
            || header.StartsWith(jpeg)
            || header.StartsWith("GIF87a"u8)
            || header.StartsWith("GIF89a"u8) // GIF
            || (header.Length >= 12 && header[..4].SequenceEqual(riff) && header[8..].StartsWith(webp)) // WebP
            || header.StartsWith(bmp)
            || header.StartsWith(ico)
            || header.StartsWith("II*\0"u8)
            || header.StartsWith("MM\0*"u8); // TIFF
    }
}
