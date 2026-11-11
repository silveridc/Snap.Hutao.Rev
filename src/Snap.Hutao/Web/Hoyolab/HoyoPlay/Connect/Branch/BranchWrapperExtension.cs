// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Rev.Web.Hoyolab.HoyoPlay.Connect.Branch;

internal static class BranchWrapperExtension
{
    extension(BranchWrapper branchWrapper)
    {
        public BranchWrapper GetTaggedCopy(string tag)
        {
            return new()
            {
                PackageId = branchWrapper.PackageId,
                Branch = branchWrapper.Branch,
                Password = branchWrapper.Password,
                Tag = tag,
            };
        }
    }
}