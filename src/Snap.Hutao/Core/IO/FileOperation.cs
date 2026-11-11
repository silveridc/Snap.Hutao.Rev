// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Rev.Win32.Foundation;
using System.IO;

namespace Snap.Hutao.Rev.Core.IO;

internal static class FileOperation
{
    public static void Copy(ValueFile source, ValueFile target, bool overwrite)
    {
        try
        {
            File.Copy(source, target, overwrite);
        }
        catch (IOException ex)
        {
            if (HRESULT.IsWin32(ex.HResult, WIN32_ERROR.ERROR_ENCRYPTION_FAILED))
            {
                using (FileStream srcStream = File.Open(source, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (FileStream destStream = new(target, overwrite ? FileMode.Create : FileMode.CreateNew))
                    {
                        srcStream.CopyTo(destStream);
                    }
                }
            }
            else
            {
                throw;
            }
        }
    }

    public static bool Move(ValueFile source, ValueFile target, bool overwrite)
    {
        if (!File.Exists(source))
        {
            return false;
        }

        if (overwrite)
        {
            try
            {
                File.Move(source, target, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        if (File.Exists(target))
        {
            return false;
        }

        try
        {
            File.Move(source, target, false);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool Delete(ValueFile file)
    {
        if (!File.Exists(file))
        {
            return false;
        }

        File.Delete(file);
        return true;
    }
}
