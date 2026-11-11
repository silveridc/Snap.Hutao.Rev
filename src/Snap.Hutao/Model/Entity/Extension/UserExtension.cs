// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Model.Entity.Extension;

internal static class UserExtension
{
    extension(User user)
    {
        public bool TryUpdateFingerprint(string? deviceFp)
        {
            if (string.IsNullOrEmpty(deviceFp))
            {
                return false;
            }

            user.Fingerprint = deviceFp;
            user.FingerprintLastUpdateTime = DateTimeOffset.UtcNow;
            return true;
        }
    }
}