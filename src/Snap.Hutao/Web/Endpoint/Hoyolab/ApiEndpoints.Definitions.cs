// Copyright (c) zhaoyj. All rights reserved.
// Licensed under the MIT license.

#pragma warning disable CS1591
#pragma warning disable SA1003, SA1009, SA1010, SA1013, SA1027, SA1028
#pragma warning disable SA1101, SA1106, SA1117, SA1122, SA1128
#pragma warning disable SA1201, SA1202, SA1205
#pragma warning disable SA1413
#pragma warning disable SA1514, SA1516
#pragma warning disable SA1611, SA1612, SA1614, SA1615, SA1616
#pragma warning disable SA1623, SA1629, SA1649

#nullable enable
namespace Snap.Hutao.Rev.Web.Endpoint.Hoyolab;
internal partial interface IApiEndpoints
{
    /// <summary>
    /// <code>CN: https://passport-api.mihoyo.com/account/ma-cn-verifier/app/createActionTicketByToken</code>
    /// <code>OS: </code>
    /// </summary>
    string AccountCreateActionTicket();
    /// <summary>
    /// <code>CN: https://passport-api.mihoyo.com/account/ma-cn-verifier/app/createAuthTicketByGameBiz</code>
    /// <code>OS: https://sg-public-api.hoyoverse.com/account/ma-verifier/api/createAuthTicketBySToken</code>
    /// </summary>
    string AccountCreateAuthTicketByGameBiz();
    /// <summary>
    /// <code>CN: </code>
    /// <code>OS: https://sg-public-api.hoyoverse.com/account/ma-verifier/api/createEmailCaptchaByActionTicket</code>
    /// </summary>
    string AccountCreateEmailCaptchaByActionTicket();
    /// <summary>
    /// <code>CN: https://passport-api.mihoyo.com/account/ma-cn-verifier/verifier/createLoginCaptcha</code>
    /// <code>OS: </code>
    /// </summary>
    string AccountCreateLoginCaptcha();
    /// <summary>
    /// <code>CN: https://passport-api.mihoyo.com/account/ma-cn-passport/app/createQRLogin</code>
    /// <code>OS: </code>
    /// </summary>
    string AccountCreateQrLogin();
    /// <summary>
    /// <code>CN: </code>
    /// <code>OS: https://sg-public-api.hoyoverse.com/account/ma-verifier/api/getActionTicketInfo</code>
    /// </summary>
    string AccountGetActionTicketInfo();
    /// <summary>
    /// <code>CN: https://passport-api.mihoyo.com/account/auth/api/getCookieAccountInfoBySToken</code>
    /// <code>OS: https://api-account-os.hoyoverse.com/account/auth/api/getCookieAccountInfoBySToken</code>
    /// </summary>
    string AccountGetCookieTokenBySToken();
    /// <summary>
    /// <code>CN: https://passport-api.mihoyo.com/account/auth/api/getLTokenBySToken</code>
    /// <code>OS: https://api-account-os.hoyoverse.com/account/auth/api/getLTokenBySToken</code>
    /// </summary>
    string AccountGetLTokenBySToken();
    /// <summary>
    /// <code>CN: https://passport-api.mihoyo.com/account/ma-cn-session/app/getTokenByGameToken</code>
    /// <code>OS: </code>
    /// </summary>
    string AccountGetSTokenByGameToken();
    /// <summary>
    /// <code>CN: https://passport-api.mihoyo.com/account/ma-cn-session/app/getTokenBySToken</code>
    /// <code>OS: </code>
    /// </summary>
    string AccountGetSTokenByOldToken();
    /// <summary>
    /// <code>CN: https://passport-api.mihoyo.com/account/ma-cn-passport/app/loginByMobileCaptcha</code>
    /// <code>OS: </code>
    /// </summary>
    string AccountLoginByMobileCaptcha();
    /// <summary>
    /// <code>CN: https://passport-api.mihoyo.com/account/ma-cn-passport/app/loginByPassword</code>
    /// <code>OS: https://sg-public-api.hoyoverse.com/account/ma-passport/api/appLoginByPassword</code>
    /// </summary>
    string AccountLoginByPassword();
    /// <summary>
    /// <code>CN: </code>
    /// <code>OS: https://sg-public-api.hoyoverse.com/account/ma-passport/api/appLoginByThirdParty</code>
    /// </summary>
    string AccountLoginByThirdParty();
    /// <summary>
    /// <code>CN: https://passport-api.mihoyo.com/account/ma-cn-passport/app/queryQRLoginStatus</code>
    /// <code>OS: </code>
    /// </summary>
    string AccountQueryQrLoginStatus();
    /// <summary>
    /// <code>CN: </code>
    /// <code>OS: https://sg-public-api.hoyoverse.com/account/ma-verifier/api/verifyActionTicketPartly</code>
    /// </summary>
    string AccountVerifyActionTicketPartly();
    /// <summary>
    /// <code>CN: https://passport-api-v4.mihoyo.com/account/ma-cn-session/web/verifyLtoken</code>
    /// <code>OS: </code>
    /// </summary>
    string AccountVerifyLtoken();
    /// <summary>
    /// <code>CN: </code>
    /// <code>OS: https://act.hoyolab.com/</code>
    /// </summary>
    string ActHoyolabReferer();
    /// <summary>
    /// <code>CN: https://hk4e-ann-api.mihoyo.com/common/hk4e_cn/announcement/api/getAnnContent?{AnnouncementQuery(languageCode, region)}</code>
    /// <code>OS: https://sg-hk4e-api.hoyoverse.com/common/hk4e_global/announcement/api/getAnnContent?{AnnouncementQuery(languageCode, region)}</code>
    /// </summary>
    string AnnContent(string languageCode, Snap.Hutao.Rev.Web.Hoyolab.Region region);
    /// <summary>
    /// <code>CN: https://hk4e-ann-api.mihoyo.com/common/hk4e_cn/announcement/api/getAnnList?{AnnouncementQuery(languageCode, region)}</code>
    /// <code>OS: https://sg-hk4e-api.hoyoverse.com/common/hk4e_global/announcement/api/getAnnList?{AnnouncementQuery(languageCode, region)}</code>
    /// </summary>
    string AnnList(string languageCode, Snap.Hutao.Rev.Web.Hoyolab.Region region);
    /// <summary>
    /// <code>CN: game=hk4e&amp;game_biz=hk4e_cn&amp;lang={languageCode}&amp;bundle_id=hk4e_cn&amp;platform=pc&amp;region={region}&amp;level=55&amp;uid=100000000</code>
    /// <code>OS: game=hk4e&amp;game_biz=hk4e_global&amp;lang={languageCode}&amp;bundle_id=hk4e_global&amp;platform=pc&amp;region={region}&amp;level=55&amp;uid=100000000</code>
    /// </summary>
    string AnnouncementQuery(string languageCode, Snap.Hutao.Rev.Web.Hoyolab.Region region);
    /// <summary>
    /// <code>CN: https://app.mihoyo.com/account/auth/api/genAuthKey</code>
    /// <code>OS: </code>
    /// </summary>
    string AppAuthGenAuthKey();
    /// <summary>
    /// <code>CN: https://app.mihoyo.com</code>
    /// <code>OS: https://app.hoyolab.com/</code>
    /// </summary>
    string AppReferer();
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/auth/api/getActionTicketBySToken?action_type={actionType}&amp;stoken={Uri.EscapeDataString(stoken)}&amp;uid={uid}</code>
    /// <code>OS: https://api-account-os.hoyoverse.com/account/auth/api/getActionTicketBySToken?action_type={actionType}&amp;stoken={Uri.EscapeDataString(stoken)}&amp;uid={uid}</code>
    /// </summary>
    string AuthActionTicket(string actionType, string stoken, string uid);
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/auth/api/getMultiTokenByLoginTicket?login_ticket={loginTicket}&amp;uid={loginUid}&amp;token_types=3</code>
    /// <code>OS: https://api-account-os.hoyoverse.com/account/auth/api/getMultiTokenByLoginTicket?login_ticket={loginTicket}&amp;uid={loginUid}&amp;token_types=3</code>
    /// </summary>
    string AuthMultiToken(string loginTicket, string loginUid);
    /// <summary>
    /// <code>CN: https://bbs.mihoyo.com/</code>
    /// <code>OS: </code>
    /// </summary>
    string BbsReferer();
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/binding/api/genAuthKey</code>
    /// <code>OS: https://api-account-os.hoyoverse.com/binding/api/genAuthKey</code>
    /// </summary>
    string BindingGenAuthKey();
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/event/e20200928calculate/v1/avatar/list</code>
    /// <code>OS: https://sg-public-api.hoyolab.com/event/e20200928calculate/v1/avatar/list</code>
    /// </summary>
    string CalculateAvatarList();
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/event/e20200928calculate/v1/avatarSkill/list?avatar_id={avatar.Id}&amp;element_attr_id={(int)avatar.ElementAttrId}</code>
    /// <code>OS: </code>
    /// </summary>
    string CalculateAvatarSkillList(Snap.Hutao.Rev.Web.Hoyolab.Takumi.Event.Calculate.Avatar avatar);
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/event/e20200928calculate/v3/batch_compute</code>
    /// <code>OS: https://sg-public-api.hoyolab.com/event/e20200928calculate/v3/batch_compute</code>
    /// </summary>
    string CalculateBatchCompute();
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/event/e20200928calculate/v2/compute</code>
    /// <code>OS: https://sg-public-api.hoyoverse.com/event/calculateos/compute</code>
    /// </summary>
    string CalculateCompute();
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/event/e20200928calculate/v1/furniture/blueprint?share_code={shareCode}</code>
    /// <code>OS: https://sg-public-api.hoyolab.com/event/e20200928calculate/v1/furniture/blueprint?share_code={shareCode}</code>
    /// </summary>
    string CalculateFurnitureBlueprint(string shareCode);
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/event/e20200928calculate/v1/furniture/compute</code>
    /// <code>OS: https://sg-public-api.hoyolab.com/event/e20200928calculate/v1/furniture/compute</code>
    /// </summary>
    string CalculateFurnitureCompute();
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/event/e20200928calculate/v1/furniture/list</code>
    /// <code>OS: https://sg-public-api.hoyolab.com/event/e20200928calculate/v1/furniture/list</code>
    /// </summary>
    string CalculateFurnitureList();
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/event/e20200928calculate/v1/sync/avatar/detail?avatar_id={avatarId.Value}&amp;{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToUidRegionQueryString(uid)}</code>
    /// <code>OS: https://sg-public-api.hoyoverse.com/event/calculateos/sync/avatar/detail?avatar_id={avatarId.Value}&amp;{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToUidRegionQueryString(uid)}</code>
    /// </summary>
    string CalculateSyncAvatarDetail(Model.Primitive.AvatarId avatarId, Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid);
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/event/e20200928calculate/v1/sync/avatar/list</code>
    /// <code>OS: https://sg-public-api.hoyolab.com/event/e20200928calculate/v1/sync/avatar/list</code>
    /// </summary>
    string CalculateSyncAvatarList();
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/event/e20200928calculate/v1/weapon/list</code>
    /// <code>OS: https://sg-public-api.hoyolab.com/event/e20200928calculate/v1/weapon/list</code>
    /// </summary>
    string CalculateWeaponList();
    /// <summary>
    /// <code>CN: https://api-takumi-record.mihoyo.com/game_record/app/card/wapi/createVerification?is_high={(highRisk ? "true" : "false")}</code>
    /// <code>OS: </code>
    /// </summary>
    string CardCreateVerification(bool highRisk);
    /// <summary>
    /// <code>CN: https://api-takumi-record.mihoyo.com/game_record/app/card/wapi/verifyVerification</code>
    /// <code>OS: </code>
    /// </summary>
    string CardVerifyVerification();
    /// <summary>
    /// <code>CN: https://api-takumi-record.mihoyo.com/game_record/app/card/api/getWidgetData?game_id=2</code>
    /// <code>OS: </code>
    /// </summary>
    string CardWidgetData();
    /// <summary>
    /// <code>CN: https://api-takumi-record.mihoyo.com/game_record/app/genshin/aapi/widget/v2?game_id=2</code>
    /// <code>OS: </code>
    /// </summary>
    string CardWidgetData2();
    /// <summary>
    /// <code>CN: https://public-data-api.mihoyo.com/device-fp/api/getExtList?platform={platform:D}</code>
    /// <code>OS: </code>
    /// </summary>
    string DeviceFpGetExtList(int platform);
    /// <summary>
    /// <code>CN: https://public-data-api.mihoyo.com/device-fp/api/getFp</code>
    /// <code>OS: </code>
    /// </summary>
    string DeviceFpGetFp();
    /// <summary>
    /// <code>CN: https://public-operation-hk4e.mihoyo.com/gacha_info/api/getGachaLog?{query}</code>
    /// <code>OS: https://public-operation-hk4e-sg.hoyoverse.com/gacha_info/api/getGachaLog?{query}</code>
    /// </summary>
    string GachaInfoGetGachaLog(string query);
    /// <summary>
    /// <code>CN: https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/character/list</code>
    /// <code>OS: https://bbs-api-os.hoyolab.com/game_record/app/genshin/api/character/list</code>
    /// </summary>
    string GameRecordCharacterList();
    /// <summary>
    /// <code>CN: https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/character/detail</code>
    /// <code>OS: https://bbs-api-os.hoyolab.com/game_record/app/genshin/api/character/detail</code>
    /// </summary>
    string GameRecordCharacterDetail();
    /// <summary>
    /// <code>CN: https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/dailyNote?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}</code>
    /// <code>OS: https://bbs-api-os.hoyolab.com/game_record/app/genshin/api/dailyNote?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}</code>
    /// </summary>
    string GameRecordDailyNote(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid);
    /// <summary>
    /// <code>CN: https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/dailyNote</code>
    /// <code>OS: </code>
    /// </summary>
    string GameRecordDailyNotePath();
    /// <summary>
    /// <code>CN: https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/hard_challenge?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}&amp;need_detail=true</code>
    /// <code>OS: https://sg-public-api.hoyolab.com/event/game_record/genshin/api/hard_challenge?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}&amp;need_detail=true</code>
    /// </summary>
    string GameRecordHardChallenge(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid);
    /// <summary>
    /// <code>CN: https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/hard_challenge</code>
    /// <code>OS: </code>
    /// </summary>
    string GameRecordHardChallengePath();
    /// <summary>
    /// <code>CN: https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/hard_challenge/popularity?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}</code>
    /// <code>OS: https://sg-public-api.hoyolab.com/event/game_record/genshin/api/hard_challenge/popularity?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}</code>
    /// </summary>
    string GameRecordHardChallengePopularity(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid);
    /// <summary>
    /// <code>CN: https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/index?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}</code>
    /// <code>OS: https://bbs-api-os.hoyolab.com/game_record/app/genshin/api/index?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}</code>
    /// </summary>
    string GameRecordIndex(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid);
    /// <summary>
    /// <code>CN: https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/index</code>
    /// <code>OS: </code>
    /// </summary>
    string GameRecordIndexPath();
    /// <summary>
    /// <code>CN: https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/roleBasicInfo?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}</code>
    /// <code>OS: https://bbs-api-os.hoyolab.com/game_record/app/genshin/api/roleBasicInfo?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}</code>
    /// </summary>
    string GameRecordRoleBasicInfo(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid);
    /// <summary>
    /// <code>CN: https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/role_combat?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}&amp;need_detail=true&amp;active=1</code>
    /// <code>OS: https://sg-public-api.hoyolab.com/event/game_record/genshin/api/role_combat?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}&amp;need_detail=true&amp;active=1</code>
    /// </summary>
    string GameRecordRoleCombat(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid);
    /// <summary>
    /// <code>CN: https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/role_combat</code>
    /// <code>OS: https://sg-public-api.hoyolab.com/event/game_record/genshin/api/role_combat</code>
    /// </summary>
    string GameRecordRoleCombatPath();
    /// <summary>
    /// <code>CN: https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/spiralAbyss?schedule_type={(int)scheduleType}&amp;{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}</code>
    /// <code>OS: https://bbs-api-os.hoyolab.com/game_record/app/genshin/api/spiralAbyss?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}&amp;schedule_type={(int)scheduleType}</code>
    /// </summary>
    string GameRecordSpiralAbyss(Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.ScheduleType scheduleType, Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid);
    /// <summary>
    /// <code>CN: https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/spiralAbyss</code>
    /// <code>OS: </code>
    /// </summary>
    string GameRecordSpiralAbyssPath();
    /// <summary>
    /// <code>CN: https://bbs-api.miyoushe.com/apihub/api/home/new?gids={gid}</code>
    /// <code>OS: </code>
    /// </summary>
    string HomeNew(int gid);
    /// <summary>
    /// <code>CN: https://hyp-api.mihoyo.com/hyp/hyp-connect/api/getGameDeprecatedFileConfigs?channel={scheme.Channel:D}&amp;game_ids[]={scheme.GameId}&amp;launcher_id={scheme.LauncherId}&amp;sub_channel={scheme.SubChannel:D}</code>
    /// <code>OS: https://sg-hyp-api.hoyoverse.com/hyp/hyp-connect/api/getGameDeprecatedFileConfigs?channel={scheme.Channel:D}&amp;game_ids[]={scheme.GameId}&amp;launcher_id={scheme.LauncherId}&amp;sub_channel={scheme.SubChannel:D}</code>
    /// </summary>
    string HoyoPlayConnectDeprecatedFileConfigs(Service.Game.Scheme.LaunchScheme scheme);
    /// <summary>
    /// <code>CN: https://hyp-api.mihoyo.com/hyp/hyp-connect/api/getGameBranches?game_ids[]={scheme.GameId}&amp;launcher_id={scheme.LauncherId}</code>
    /// <code>OS: https://sg-hyp-api.hoyoverse.com/hyp/hyp-connect/api/getGameBranches?game_ids[]={scheme.GameId}&amp;launcher_id={scheme.LauncherId}</code>
    /// </summary>
    string HoyoPlayConnectGameBranches(Service.Game.Scheme.LaunchScheme scheme);
    /// <summary>
    /// <code>CN: https://hyp-api.mihoyo.com/hyp/hyp-connect/api/getGameChannelSDKs?channel={scheme.Channel:D}&amp;game_ids[]={scheme.GameId}&amp;launcher_id={scheme.LauncherId}&amp;sub_channel={scheme.SubChannel:D}</code>
    /// <code>OS: https://sg-hyp-api.hoyoverse.com/hyp/hyp-connect/api/getGameChannelSDKs?channel={scheme.Channel:D}&amp;game_ids[]={scheme.GameId}&amp;launcher_id={scheme.LauncherId}&amp;sub_channel={scheme.SubChannel:D}</code>
    /// </summary>
    string HoyoPlayConnectGameChannelSDKs(Service.Game.Scheme.LaunchScheme scheme);
    /// <summary>
    /// <code>CN: https://hyp-api.mihoyo.com/hyp/hyp-connect/api/getGamePackages?game_ids[]={scheme.GameId}&amp;launcher_id={scheme.LauncherId}</code>
    /// <code>OS: https://sg-hyp-api.hoyoverse.com/hyp/hyp-connect/api/getGamePackages?game_ids[]={scheme.GameId}&amp;launcher_id={scheme.LauncherId}</code>
    /// </summary>
    string HoyoPlayConnectGamePackages(Service.Game.Scheme.LaunchScheme scheme);
    /// <summary>
    /// <code>CN: e202311201442471</code>
    /// <code>OS: e202102251931481</code>
    /// </summary>
    string LunaSolActivityId();
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/event/luna/home?lang={languageCode}&amp;act_id={LunaSolActivityId()}</code>
    /// <code>OS: https://sg-hk4e-api.hoyoverse.com/event/sol/home?lang={languageCode}&amp;act_id={LunaSolActivityId()}</code>
    /// </summary>
    string LunaSolHome(string languageCode);
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/event/luna/info?lang={languageCode}&amp;act_id={LunaSolActivityId()}&amp;{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToUidRegionQueryString(uid)}</code>
    /// <code>OS: https://sg-hk4e-api.hoyoverse.com/event/sol/info?lang={languageCode}&amp;act_id={LunaSolActivityId()}&amp;{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToUidRegionQueryString(uid)}</code>
    /// </summary>
    string LunaSolInfo(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid, string languageCode);
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/event/luna/resign</code>
    /// <code>OS: https://sg-hk4e-api.hoyoverse.com/event/sol/resign?lang={languageCode}</code>
    /// </summary>
    string LunaSolReSign(string languageCode);
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/event/luna/resign_info?act_id={LunaSolActivityId()}&amp;{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToUidRegionQueryString(uid)}</code>
    /// <code>OS: https://sg-hk4e-api.hoyoverse.com/event/sol/resign_info?lang={languageCode}&amp;act_id={LunaSolActivityId()}&amp;{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToUidRegionQueryString(uid)}</code>
    /// </summary>
    string LunaSolResignInfo(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid, string languageCode);
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/event/luna/sign</code>
    /// <code>OS: https://sg-hk4e-api.hoyoverse.com/event/sol/sign?lang=zh-cn</code>
    /// </summary>
    string LunaSolSign();
    /// <summary>
    /// <code>CN: https://api-takumi-static.mihoyo.com/event/miyolive/refreshCode?time={DateTimeOffset.Now.ToUnixTimeSeconds()}</code>
    /// <code>OS: </code>
    /// </summary>
    string MiyoliveRefreshCode();
    /// <summary>
    /// <code>CN: https://hk4e-sdk.mihoyo.com/hk4e_cn/combo/panda/qrcode/fetch</code>
    /// <code>OS: </code>
    /// </summary>
    string QrCodeFetch();
    /// <summary>
    /// <code>CN: https://hk4e-sdk.mihoyo.com/hk4e_cn/combo/panda/qrcode/query</code>
    /// <code>OS: </code>
    /// </summary>
    string QrCodeQuery();
    /// <summary>
    /// <code>CN: https://downloader-api.mihoyo.com/downloader/sophon_chunk/api/getBuild?branch={branch.Branch}&amp;package_id={branch.PackageId}&amp;password={branch.Password}&amp;tag={branch.Tag}</code>
    /// <code>OS: https://sg-downloader-api.hoyoverse.com/downloader/sophon_chunk/api/getBuild?branch={branch.Branch}&amp;package_id={branch.PackageId}&amp;password={branch.Password}&amp;tag={branch.Tag}</code>
    /// </summary>
    string SophonChunkGetBuild(Snap.Hutao.Rev.Web.Hoyolab.HoyoPlay.Connect.Branch.BranchWrapper branch);
    /// <summary>
    /// <code>CN: https://downloader-api.mihoyo.com/downloader/sophon_chunk/api/getBuild?branch={branch.Branch}&amp;package_id={branch.PackageId}&amp;password={branch.Password}</code>
    /// <code>OS: https://sg-downloader-api.hoyoverse.com/downloader/sophon_chunk/api/getBuild?branch={branch.Branch}&amp;package_id={branch.PackageId}&amp;password={branch.Password}</code>
    /// </summary>
    string SophonChunkGetBuildNoTag(Snap.Hutao.Rev.Web.Hoyolab.HoyoPlay.Connect.Branch.BranchWrapper branch);
    /// <summary>
    /// <code>CN: https://downloader-api.mihoyo.com/downloader/sophon_chunk/api/getPatchBuild</code>
    /// <code>OS: https://sg-downloader-api.hoyoverse.com/downloader/sophon_chunk/api/getPatchBuild</code>
    /// </summary>
    string SophonChunkGetPatchBuild();
    /// <summary>
    /// <code>CN: https://bbs-api.mihoyo.com/user/wapi/getUserFullInfo?gids=2</code>
    /// <code>OS: https://bbs-api-os.hoyolab.com/community/user/wapi/getUserFullInfo?gid=2</code>
    /// </summary>
    string UserFullInfo();
    /// <summary>
    /// <code>CN: https://bbs-api.mihoyo.com/user/wapi/getUserFullInfo?uid={accountId}&amp;gids=2</code>
    /// <code>OS: https://bbs-api-os.hoyolab.com/community/painter/wapi/user/full</code>
    /// </summary>
    string UserFullInfoQuery(string accountId);
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/binding/api/getUserGameRoles?action_ticket={actionTicket}&amp;game_biz=hk4e_cn</code>
    /// <code>OS: </code>
    /// </summary>
    string UserGameRolesByActionTicket(string actionTicket);
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/binding/api/getUserGameRolesByCookie?game_biz=hk4e_cn</code>
    /// <code>OS: https://api-os-takumi.hoyoverse.com/binding/api/getUserGameRolesByCookie?game_biz=hk4e_global</code>
    /// </summary>
    string UserGameRolesByCookie();
    /// <summary>
    /// <code>CN: </code>
    /// <code>OS: https://api-account-os.hoyoverse.com/binding/api/getUserGameRolesByLtoken?game_biz=hk4e_global&amp;region={region}</code>
    /// </summary>
    string UserGameRolesByLtoken(Snap.Hutao.Rev.Web.Hoyolab.Region region);
    /// <summary>
    /// <code>CN: https://api-takumi.mihoyo.com/binding/api/getUserGameRolesByStoken</code>
    /// <code>OS: </code>
    /// </summary>
    string UserGameRolesBySToken();
    /// <summary>
    /// <code>CN: </code>
    /// <code>OS: https://webapi-os.account.hoyoverse.com/Api/login_by_cookie</code>
    /// </summary>
    string WebApiOsAccountLoginByCookie();
    /// <summary>
    /// <code>CN: </code>
    /// <code>OS: https://api-account-os.hoyoverse.com/account/auth/api/webLoginByPassword</code>
    /// </summary>
    string WebLoginByPassword();
    /// <summary>
    /// <code>CN: https://webstatic.mihoyo.com</code>
    /// <code>OS: https://webstatic-sea.mihoyo.com</code>
    /// </summary>
    string WebStaticReferer();
}

internal abstract class ApiEndpointsImplementationForChinese : IApiEndpoints
{
    public string AccountCreateActionTicket() => $"https://passport-api.mihoyo.com/account/ma-cn-verifier/app/createActionTicketByToken";
    public string AccountCreateAuthTicketByGameBiz() => $"https://passport-api.mihoyo.com/account/ma-cn-verifier/app/createAuthTicketByGameBiz";
    public string AccountCreateEmailCaptchaByActionTicket() => throw new NotSupportedException();
    public string AccountCreateLoginCaptcha() => $"https://passport-api.mihoyo.com/account/ma-cn-verifier/verifier/createLoginCaptcha";
    public string AccountCreateQrLogin() => $"https://passport-api.mihoyo.com/account/ma-cn-passport/app/createQRLogin";
    public string AccountGetActionTicketInfo() => throw new NotSupportedException();
    public string AccountGetCookieTokenBySToken() => $"https://passport-api.mihoyo.com/account/auth/api/getCookieAccountInfoBySToken";
    public string AccountGetLTokenBySToken() => $"https://passport-api.mihoyo.com/account/auth/api/getLTokenBySToken";
    public string AccountGetSTokenByGameToken() => $"https://passport-api.mihoyo.com/account/ma-cn-session/app/getTokenByGameToken";
    public string AccountGetSTokenByOldToken() => $"https://passport-api.mihoyo.com/account/ma-cn-session/app/getTokenBySToken";
    public string AccountLoginByMobileCaptcha() => $"https://passport-api.mihoyo.com/account/ma-cn-passport/app/loginByMobileCaptcha";
    public string AccountLoginByPassword() => $"https://passport-api.mihoyo.com/account/ma-cn-passport/app/loginByPassword";
    public string AccountLoginByThirdParty() => throw new NotSupportedException();
    public string AccountQueryQrLoginStatus() => $"https://passport-api.mihoyo.com/account/ma-cn-passport/app/queryQRLoginStatus";
    public string AccountVerifyActionTicketPartly() => throw new NotSupportedException();
    public string AccountVerifyLtoken() => $"https://passport-api-v4.mihoyo.com/account/ma-cn-session/web/verifyLtoken";
    public string ActHoyolabReferer() => throw new NotSupportedException();
    public string AnnContent(string languageCode, Snap.Hutao.Rev.Web.Hoyolab.Region region) => $"https://hk4e-ann-api.mihoyo.com/common/hk4e_cn/announcement/api/getAnnContent?{AnnouncementQuery(languageCode, region)}";
    public string AnnList(string languageCode, Snap.Hutao.Rev.Web.Hoyolab.Region region) => $"https://hk4e-ann-api.mihoyo.com/common/hk4e_cn/announcement/api/getAnnList?{AnnouncementQuery(languageCode, region)}";
    public string AnnouncementQuery(string languageCode, Snap.Hutao.Rev.Web.Hoyolab.Region region) => $"game=hk4e&game_biz=hk4e_cn&lang={languageCode}&bundle_id=hk4e_cn&platform=pc&region={region}&level=55&uid=100000000";
    public string AppAuthGenAuthKey() => $"https://app.mihoyo.com/account/auth/api/genAuthKey";
    public string AppReferer() => $"https://app.mihoyo.com";
    public string AuthActionTicket(string actionType, string stoken, string uid) => $"https://api-takumi.mihoyo.com/auth/api/getActionTicketBySToken?action_type={actionType}&stoken={Uri.EscapeDataString(stoken)}&uid={uid}";
    public string AuthMultiToken(string loginTicket, string loginUid) => $"https://api-takumi.mihoyo.com/auth/api/getMultiTokenByLoginTicket?login_ticket={loginTicket}&uid={loginUid}&token_types=3";
    public string BbsReferer() => $"https://bbs.mihoyo.com/";
    public string BindingGenAuthKey() => $"https://api-takumi.mihoyo.com/binding/api/genAuthKey";
    public string CalculateAvatarList() => $"https://api-takumi.mihoyo.com/event/e20200928calculate/v1/avatar/list";
    public string CalculateAvatarSkillList(Snap.Hutao.Rev.Web.Hoyolab.Takumi.Event.Calculate.Avatar avatar) => $"https://api-takumi.mihoyo.com/event/e20200928calculate/v1/avatarSkill/list?avatar_id={avatar.Id}&element_attr_id={(int)avatar.ElementAttrId}";
    public string CalculateBatchCompute() => $"https://api-takumi.mihoyo.com/event/e20200928calculate/v3/batch_compute";
    public string CalculateCompute() => $"https://api-takumi.mihoyo.com/event/e20200928calculate/v2/compute";
    public string CalculateFurnitureBlueprint(string shareCode) => $"https://api-takumi.mihoyo.com/event/e20200928calculate/v1/furniture/blueprint?share_code={shareCode}";
    public string CalculateFurnitureCompute() => $"https://api-takumi.mihoyo.com/event/e20200928calculate/v1/furniture/compute";
    public string CalculateFurnitureList() => $"https://api-takumi.mihoyo.com/event/e20200928calculate/v1/furniture/list";
    public string CalculateSyncAvatarDetail(Model.Primitive.AvatarId avatarId, Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid) => $"https://api-takumi.mihoyo.com/event/e20200928calculate/v1/sync/avatar/detail?avatar_id={avatarId.Value}&{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToUidRegionQueryString(uid)}";
    public string CalculateSyncAvatarList() => $"https://api-takumi.mihoyo.com/event/e20200928calculate/v1/sync/avatar/list";
    public string CalculateWeaponList() => $"https://api-takumi.mihoyo.com/event/e20200928calculate/v1/weapon/list";
    public string CardCreateVerification(bool highRisk) => $"https://api-takumi-record.mihoyo.com/game_record/app/card/wapi/createVerification?is_high={(highRisk ? "true" : "false")}";
    public string CardVerifyVerification() => $"https://api-takumi-record.mihoyo.com/game_record/app/card/wapi/verifyVerification";
    public string CardWidgetData() => $"https://api-takumi-record.mihoyo.com/game_record/app/card/api/getWidgetData?game_id=2";
    public string CardWidgetData2() => $"https://api-takumi-record.mihoyo.com/game_record/app/genshin/aapi/widget/v2?game_id=2";
    public string DeviceFpGetExtList(int platform) => $"https://public-data-api.mihoyo.com/device-fp/api/getExtList?platform={platform:D}";
    public string DeviceFpGetFp() => $"https://public-data-api.mihoyo.com/device-fp/api/getFp";
    public string GachaInfoGetGachaLog(string query) => $"https://public-operation-hk4e.mihoyo.com/gacha_info/api/getGachaLog?{query}";
    public string GameRecordCharacterList() => $"https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/character/list";
    public string GameRecordCharacterDetail() => $"https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/character/detail";
    public string GameRecordDailyNote(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid) => $"https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/dailyNote?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}";
    public string GameRecordDailyNotePath() => $"https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/dailyNote";
    public string GameRecordHardChallenge(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid) => $"https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/hard_challenge?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}&need_detail=true";
    public string GameRecordHardChallengePath() => $"https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/hard_challenge";
    public string GameRecordHardChallengePopularity(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid) => $"https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/hard_challenge/popularity?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}";
    public string GameRecordIndex(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid) => $"https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/index?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}";
    public string GameRecordIndexPath() => $"https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/index";
    public string GameRecordRoleBasicInfo(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid) => $"https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/roleBasicInfo?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}";
    public string GameRecordRoleCombat(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid) => $"https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/role_combat?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}&need_detail=true&active=1";
    public string GameRecordRoleCombatPath() => $"https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/role_combat";
    public string GameRecordSpiralAbyss(Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.ScheduleType scheduleType, Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid) => $"https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/spiralAbyss?schedule_type={(int)scheduleType}&{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}";
    public string GameRecordSpiralAbyssPath() => $"https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/spiralAbyss";
    public string HomeNew(int gid) => $"https://bbs-api.miyoushe.com/apihub/api/home/new?gids={gid}";
    public string HoyoPlayConnectDeprecatedFileConfigs(Service.Game.Scheme.LaunchScheme scheme) => $"https://hyp-api.mihoyo.com/hyp/hyp-connect/api/getGameDeprecatedFileConfigs?channel={scheme.Channel:D}&game_ids[]={scheme.GameId}&launcher_id={scheme.LauncherId}&sub_channel={scheme.SubChannel:D}";
    public string HoyoPlayConnectGameBranches(Service.Game.Scheme.LaunchScheme scheme) => $"https://hyp-api.mihoyo.com/hyp/hyp-connect/api/getGameBranches?game_ids[]={scheme.GameId}&launcher_id={scheme.LauncherId}";
    public string HoyoPlayConnectGameChannelSDKs(Service.Game.Scheme.LaunchScheme scheme) => $"https://hyp-api.mihoyo.com/hyp/hyp-connect/api/getGameChannelSDKs?channel={scheme.Channel:D}&game_ids[]={scheme.GameId}&launcher_id={scheme.LauncherId}&sub_channel={scheme.SubChannel:D}";
    public string HoyoPlayConnectGamePackages(Service.Game.Scheme.LaunchScheme scheme) => $"https://hyp-api.mihoyo.com/hyp/hyp-connect/api/getGamePackages?game_ids[]={scheme.GameId}&launcher_id={scheme.LauncherId}";
    public string LunaSolActivityId() => $"e202311201442471";
    public string LunaSolHome(string languageCode) => $"https://api-takumi.mihoyo.com/event/luna/home?lang={languageCode}&act_id={LunaSolActivityId()}";
    public string LunaSolInfo(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid, string languageCode) => $"https://api-takumi.mihoyo.com/event/luna/info?lang={languageCode}&act_id={LunaSolActivityId()}&{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToUidRegionQueryString(uid)}";
    public string LunaSolReSign(string languageCode) => $"https://api-takumi.mihoyo.com/event/luna/resign";
    public string LunaSolResignInfo(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid, string languageCode) => $"https://api-takumi.mihoyo.com/event/luna/resign_info?act_id={LunaSolActivityId()}&{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToUidRegionQueryString(uid)}";
    public string LunaSolSign() => $"https://api-takumi.mihoyo.com/event/luna/sign";
    public string MiyoliveRefreshCode() => $"https://api-takumi-static.mihoyo.com/event/miyolive/refreshCode?time={DateTimeOffset.Now.ToUnixTimeSeconds()}";
    public string QrCodeFetch() => $"https://hk4e-sdk.mihoyo.com/hk4e_cn/combo/panda/qrcode/fetch";
    public string QrCodeQuery() => $"https://hk4e-sdk.mihoyo.com/hk4e_cn/combo/panda/qrcode/query";
    public string SophonChunkGetBuild(Snap.Hutao.Rev.Web.Hoyolab.HoyoPlay.Connect.Branch.BranchWrapper branch) => $"https://downloader-api.mihoyo.com/downloader/sophon_chunk/api/getBuild?branch={branch.Branch}&package_id={branch.PackageId}&password={branch.Password}&tag={branch.Tag}";
    public string SophonChunkGetBuildNoTag(Snap.Hutao.Rev.Web.Hoyolab.HoyoPlay.Connect.Branch.BranchWrapper branch) => $"https://downloader-api.mihoyo.com/downloader/sophon_chunk/api/getBuild?branch={branch.Branch}&package_id={branch.PackageId}&password={branch.Password}";
    public string SophonChunkGetPatchBuild() => $"https://downloader-api.mihoyo.com/downloader/sophon_chunk/api/getPatchBuild";
    public string UserFullInfo() => $"https://bbs-api.mihoyo.com/user/wapi/getUserFullInfo?gids=2";
    public string UserFullInfoQuery(string accountId) => $"https://bbs-api.mihoyo.com/user/wapi/getUserFullInfo?uid={accountId}&gids=2";
    public string UserGameRolesByActionTicket(string actionTicket) => $"https://api-takumi.mihoyo.com/binding/api/getUserGameRoles?action_ticket={actionTicket}&game_biz=hk4e_cn";
    public string UserGameRolesByCookie() => $"https://api-takumi.mihoyo.com/binding/api/getUserGameRolesByCookie?game_biz=hk4e_cn";
    public string UserGameRolesByLtoken(Snap.Hutao.Rev.Web.Hoyolab.Region region) => throw new NotSupportedException();
    public string UserGameRolesBySToken() => $"https://api-takumi.mihoyo.com/binding/api/getUserGameRolesByStoken";
    public string WebApiOsAccountLoginByCookie() => throw new NotSupportedException();
    public string WebLoginByPassword() => throw new NotSupportedException();
    public string WebStaticReferer() => $"https://webstatic.mihoyo.com";
}

internal abstract class ApiEndpointsImplementationForOversea : IApiEndpoints
{
    public string AccountCreateActionTicket() => throw new NotSupportedException();
    public string AccountCreateAuthTicketByGameBiz() => $"https://sg-public-api.hoyoverse.com/account/ma-verifier/api/createAuthTicketBySToken";
    public string AccountCreateEmailCaptchaByActionTicket() => $"https://sg-public-api.hoyoverse.com/account/ma-verifier/api/createEmailCaptchaByActionTicket";
    public string AccountCreateLoginCaptcha() => throw new NotSupportedException();
    public string AccountCreateQrLogin() => throw new NotSupportedException();
    public string AccountGetActionTicketInfo() => $"https://sg-public-api.hoyoverse.com/account/ma-verifier/api/getActionTicketInfo";
    public string AccountGetCookieTokenBySToken() => $"https://api-account-os.hoyoverse.com/account/auth/api/getCookieAccountInfoBySToken";
    public string AccountGetLTokenBySToken() => $"https://api-account-os.hoyoverse.com/account/auth/api/getLTokenBySToken";
    public string AccountGetSTokenByGameToken() => throw new NotSupportedException();
    public string AccountGetSTokenByOldToken() => throw new NotSupportedException();
    public string AccountLoginByMobileCaptcha() => throw new NotSupportedException();
    public string AccountLoginByPassword() => $"https://sg-public-api.hoyoverse.com/account/ma-passport/api/appLoginByPassword";
    public string AccountLoginByThirdParty() => $"https://sg-public-api.hoyoverse.com/account/ma-passport/api/appLoginByThirdParty";
    public string AccountQueryQrLoginStatus() => throw new NotSupportedException();
    public string AccountVerifyActionTicketPartly() => $"https://sg-public-api.hoyoverse.com/account/ma-verifier/api/verifyActionTicketPartly";
    public string AccountVerifyLtoken() => throw new NotSupportedException();
    public string ActHoyolabReferer() => $"https://act.hoyolab.com/";
    public string AnnContent(string languageCode, Snap.Hutao.Rev.Web.Hoyolab.Region region) => $"https://sg-hk4e-api.hoyoverse.com/common/hk4e_global/announcement/api/getAnnContent?{AnnouncementQuery(languageCode, region)}";
    public string AnnList(string languageCode, Snap.Hutao.Rev.Web.Hoyolab.Region region) => $"https://sg-hk4e-api.hoyoverse.com/common/hk4e_global/announcement/api/getAnnList?{AnnouncementQuery(languageCode, region)}";
    public string AnnouncementQuery(string languageCode, Snap.Hutao.Rev.Web.Hoyolab.Region region) => $"game=hk4e&game_biz=hk4e_global&lang={languageCode}&bundle_id=hk4e_global&platform=pc&region={region}&level=55&uid=100000000";
    public string AppAuthGenAuthKey() => throw new NotSupportedException();
    public string AppReferer() => $"https://app.hoyolab.com/";
    public string AuthActionTicket(string actionType, string stoken, string uid) => $"https://api-account-os.hoyoverse.com/account/auth/api/getActionTicketBySToken?action_type={actionType}&stoken={Uri.EscapeDataString(stoken)}&uid={uid}";
    public string AuthMultiToken(string loginTicket, string loginUid) => $"https://api-account-os.hoyoverse.com/account/auth/api/getMultiTokenByLoginTicket?login_ticket={loginTicket}&uid={loginUid}&token_types=3";
    public string BbsReferer() => throw new NotSupportedException();
    public string BindingGenAuthKey() => $"https://api-account-os.hoyoverse.com/binding/api/genAuthKey";
    public string CalculateAvatarList() => $"https://sg-public-api.hoyolab.com/event/e20200928calculate/v1/avatar/list";
    public string CalculateAvatarSkillList(Snap.Hutao.Rev.Web.Hoyolab.Takumi.Event.Calculate.Avatar avatar) => throw new NotSupportedException();
    public string CalculateBatchCompute() => $"https://sg-public-api.hoyolab.com/event/e20200928calculate/v3/batch_compute";
    public string CalculateCompute() => $"https://sg-public-api.hoyoverse.com/event/calculateos/compute";
    public string CalculateFurnitureBlueprint(string shareCode) => $"https://sg-public-api.hoyolab.com/event/e20200928calculate/v1/furniture/blueprint?share_code={shareCode}";
    public string CalculateFurnitureCompute() => $"https://sg-public-api.hoyolab.com/event/e20200928calculate/v1/furniture/compute";
    public string CalculateFurnitureList() => $"https://sg-public-api.hoyolab.com/event/e20200928calculate/v1/furniture/list";
    public string CalculateSyncAvatarDetail(Model.Primitive.AvatarId avatarId, Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid) => $"https://sg-public-api.hoyoverse.com/event/calculateos/sync/avatar/detail?avatar_id={avatarId.Value}&{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToUidRegionQueryString(uid)}";
    public string CalculateSyncAvatarList() => $"https://sg-public-api.hoyolab.com/event/e20200928calculate/v1/sync/avatar/list";
    public string CalculateWeaponList() => $"https://sg-public-api.hoyolab.com/event/e20200928calculate/v1/weapon/list";
    public string CardCreateVerification(bool highRisk) => throw new NotSupportedException();
    public string CardVerifyVerification() => throw new NotSupportedException();
    public string CardWidgetData() => throw new NotSupportedException();
    public string CardWidgetData2() => throw new NotSupportedException();
    public string DeviceFpGetExtList(int platform) => throw new NotSupportedException();
    public string DeviceFpGetFp() => throw new NotSupportedException();
    public string GachaInfoGetGachaLog(string query) => $"https://public-operation-hk4e-sg.hoyoverse.com/gacha_info/api/getGachaLog?{query}";
    public string GameRecordCharacterList() => $"https://bbs-api-os.hoyolab.com/game_record/app/genshin/api/character/list";
    public string GameRecordCharacterDetail() => $"https://bbs-api-os.hoyolab.com/game_record/app/genshin/api/character/detail";
    public string GameRecordDailyNote(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid) => $"https://bbs-api-os.hoyolab.com/game_record/app/genshin/api/dailyNote?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}";
    public string GameRecordDailyNotePath() => throw new NotSupportedException();
    public string GameRecordHardChallenge(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid) => $"https://sg-public-api.hoyolab.com/event/game_record/genshin/api/hard_challenge?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}&need_detail=true";
    public string GameRecordHardChallengePath() => throw new NotSupportedException();
    public string GameRecordHardChallengePopularity(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid) => $"https://sg-public-api.hoyolab.com/event/game_record/genshin/api/hard_challenge/popularity?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}";
    public string GameRecordIndex(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid) => $"https://bbs-api-os.hoyolab.com/game_record/app/genshin/api/index?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}";
    public string GameRecordIndexPath() => throw new NotSupportedException();
    public string GameRecordRoleBasicInfo(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid) => $"https://bbs-api-os.hoyolab.com/game_record/app/genshin/api/roleBasicInfo?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}";
    public string GameRecordRoleCombat(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid) => $"https://sg-public-api.hoyolab.com/event/game_record/genshin/api/role_combat?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}&need_detail=true&active=1";
    public string GameRecordRoleCombatPath() => $"https://sg-public-api.hoyolab.com/event/game_record/genshin/api/role_combat";
    public string GameRecordSpiralAbyss(Snap.Hutao.Rev.Web.Hoyolab.Takumi.GameRecord.ScheduleType scheduleType, Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid) => $"https://bbs-api-os.hoyolab.com/game_record/app/genshin/api/spiralAbyss?{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToRoleIdServerQueryString(uid)}&schedule_type={(int)scheduleType}";
    public string GameRecordSpiralAbyssPath() => throw new NotSupportedException();
    public string HomeNew(int gid) => throw new NotSupportedException();
    public string HoyoPlayConnectDeprecatedFileConfigs(Service.Game.Scheme.LaunchScheme scheme) => $"https://sg-hyp-api.hoyoverse.com/hyp/hyp-connect/api/getGameDeprecatedFileConfigs?channel={scheme.Channel:D}&game_ids[]={scheme.GameId}&launcher_id={scheme.LauncherId}&sub_channel={scheme.SubChannel:D}";
    public string HoyoPlayConnectGameBranches(Service.Game.Scheme.LaunchScheme scheme) => $"https://sg-hyp-api.hoyoverse.com/hyp/hyp-connect/api/getGameBranches?game_ids[]={scheme.GameId}&launcher_id={scheme.LauncherId}";
    public string HoyoPlayConnectGameChannelSDKs(Service.Game.Scheme.LaunchScheme scheme) => $"https://sg-hyp-api.hoyoverse.com/hyp/hyp-connect/api/getGameChannelSDKs?channel={scheme.Channel:D}&game_ids[]={scheme.GameId}&launcher_id={scheme.LauncherId}&sub_channel={scheme.SubChannel:D}";
    public string HoyoPlayConnectGamePackages(Service.Game.Scheme.LaunchScheme scheme) => $"https://sg-hyp-api.hoyoverse.com/hyp/hyp-connect/api/getGamePackages?game_ids[]={scheme.GameId}&launcher_id={scheme.LauncherId}";
    public string LunaSolActivityId() => $"e202102251931481";
    public string LunaSolHome(string languageCode) => $"https://sg-hk4e-api.hoyoverse.com/event/sol/home?lang={languageCode}&act_id={LunaSolActivityId()}";
    public string LunaSolInfo(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid, string languageCode) => $"https://sg-hk4e-api.hoyoverse.com/event/sol/info?lang={languageCode}&act_id={LunaSolActivityId()}&{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToUidRegionQueryString(uid)}";
    public string LunaSolReSign(string languageCode) => $"https://sg-hk4e-api.hoyoverse.com/event/sol/resign?lang={languageCode}";
    public string LunaSolResignInfo(Snap.Hutao.Rev.Web.Hoyolab.PlayerUid uid, string languageCode) => $"https://sg-hk4e-api.hoyoverse.com/event/sol/resign_info?lang={languageCode}&act_id={LunaSolActivityId()}&{Snap.Hutao.Rev.Web.Hoyolab.PlayerUidExtension.ToUidRegionQueryString(uid)}";
    public string LunaSolSign() => $"https://sg-hk4e-api.hoyoverse.com/event/sol/sign?lang=zh-cn";
    public string MiyoliveRefreshCode() => throw new NotSupportedException();
    public string QrCodeFetch() => throw new NotSupportedException();
    public string QrCodeQuery() => throw new NotSupportedException();
    public string SophonChunkGetBuild(Snap.Hutao.Rev.Web.Hoyolab.HoyoPlay.Connect.Branch.BranchWrapper branch) => $"https://sg-downloader-api.hoyoverse.com/downloader/sophon_chunk/api/getBuild?branch={branch.Branch}&package_id={branch.PackageId}&password={branch.Password}&tag={branch.Tag}";
    public string SophonChunkGetBuildNoTag(Snap.Hutao.Rev.Web.Hoyolab.HoyoPlay.Connect.Branch.BranchWrapper branch) => $"https://sg-downloader-api.hoyoverse.com/downloader/sophon_chunk/api/getBuild?branch={branch.Branch}&package_id={branch.PackageId}&password={branch.Password}";
    public string SophonChunkGetPatchBuild() => $"https://sg-downloader-api.hoyoverse.com/downloader/sophon_chunk/api/getPatchBuild";
    public string UserFullInfo() => $"https://bbs-api-os.hoyolab.com/community/user/wapi/getUserFullInfo?gid=2";
    public string UserFullInfoQuery(string accountId) => $"https://bbs-api-os.hoyolab.com/community/painter/wapi/user/full";
    public string UserGameRolesByActionTicket(string actionTicket) => throw new NotSupportedException();
    public string UserGameRolesByCookie() => $"https://api-os-takumi.hoyoverse.com/binding/api/getUserGameRolesByCookie?game_biz=hk4e_global";
    public string UserGameRolesByLtoken(Snap.Hutao.Rev.Web.Hoyolab.Region region) => $"https://api-account-os.hoyoverse.com/binding/api/getUserGameRolesByLtoken?game_biz=hk4e_global&region={region}";
    public string UserGameRolesBySToken() => throw new NotSupportedException();
    public string WebApiOsAccountLoginByCookie() => $"https://webapi-os.account.hoyoverse.com/Api/login_by_cookie";
    public string WebLoginByPassword() => $"https://api-account-os.hoyoverse.com/account/auth/api/webLoginByPassword";
    public string WebStaticReferer() => $"https://webstatic-sea.mihoyo.com";
}