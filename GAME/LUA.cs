using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace _i
{
    class LUA
    {
        private Game game;

        public LUA(Game game)
        {
            this.game = game;
        }
        public void CloseBank()
        {
            DoString("setmetatable(_G, { __index = Bank_Env}); this:Hide(); setmetatable(_G, { __index = BigBank_Env}); this:Hide();");
        }

        public int Index
        {
            set
            {
                Win.PostMessage(game.Handle, Global.HookMessage, value, 56);
            }
        }

        public void DestroyTeam()
        {
            DoString("if Player:IsInTeam() == 1 then Player:LeaveTeam() elseif Player:IsInRaid() == 1 then Player:LeaveRiad() end");
        }

        public void GameProduceLoginMoveToCharacter(int index)
        {
 
            DoString("GameProduceLogin:MoveToCharacter(" + index + ");");
        }

        public void QuestFrameMissionComplete(int index)
        {
            DoString("QuestFrameMissionComplete(" + index + ");");
        }

        public void QuestFrameOptionClicked(int option1, int option2)
        {
            DoString("QuestFrameOptionClicked(-1," + option1 + ", " + option2 + ");");
        }

        public void SelectRoleEnterGame()
        {

            DoString("SelectRole_EnterGame();");
        }

        public void TheFireStove_FireButton_OnClick()
        {
            DoString("setmetatable(_G, {__index = TheFireStove_Env }); TheFireStove_FireButton_OnClick();");
        }

        public void TheFireStove_StoneButton_OnClick()
        {
            DoString("setmetatable(_G, {__index = TheFireStove_Env }); TheFireStove_StoneButton_OnClick();");
        }

        public void TheFireStove_MessageBox_OK_Clicked()
        {
            DoString("setmetatable(_G, { __index = TheFireStove_MessageBox_Env }); TheFireStove_MessageBox_OK_Clicked();");
        }

        public void Play_Ani(int index)
        {
            DoString("setmetatable(_G, {__index = XingYun_Env }); if this:IsVisible() then setmetatable(_G, {__index = XingYun_Env }); Play_Ani(" + index + "); end");
        }

        public void LogOnSelectTail(int index)
        {



            DoString("setmetatable(_G, {__index = LoginLogOn_Env}); LogOn_Region:SetCurrentSelect(" + index + ");");
        }

        public void DataPoolReConnect()
        {
            DoString("DataPool:ReConnect();");
        }

        public void LogOn_ExitToSelectServer()
        {

            DoString("setmetatable(_G, {__index = LoginSelectServerQuest_Env}); SelectServerQuest_Bn1Click(); LogOn_ExitToSelectServer();");
        }

        public void SelectServer(int index)
        {

            Index = index;
            Win.PostMessage(game.Handle, Global.HookMessage, 45, 105);
        }

        public void TogleMissionOutline()
        {
            DoString("PushEvent('TOGLE_MISSION_OUTLINE');");
        }

        public void AskRet2SelServer()
        {

            DoString("AskRet2SelServer();");
        }

        public void PlayerCreateTeamSelf()
        {
            DoString("Player:CreateTeamSelf();");
        }

        public void OpenWindowMissionTrack()
        {
            DoString("OpenWindow('MissionTrack');");
        }

        public void HuoDongRiChengNextClick()
        {

            DoString("HuoDongRiCheng_next_click();");
            
        }

        public void LoginOverTime()
        {
                DoString("setmetatable(_G, {__index = TextValidate_Env}); TextValidate_BtnCloseClick(); setmetatable(_G, {__index = LoginOverTime_Env}); LoginOverTime_Bn1Click();");
        }

        public void YuanbaoShop(int list, int shop)
        {
            Win.PostMessage(game.Handle, Global.HookMessage, list, 57);
            Win.PostMessage(game.Handle, Global.HookMessage, shop, 58);
            Win.PostMessage(game.Handle, Global.HookMessage, 52, 105);
        }

        public void ToggleYuanbaoShop()
        {
            //game.LuaDoOneLineString("setmetatable(_G, {__index = YuanbaoShop_Env}); this:Show();");
            DoString("PushEvent('TOGGLE_YUANBAOSHOP');");
        }

        public void HideYuanbaoShop()
        {
            game.DoStringEx("setmetatable(_G, {__index = YuanbaoShop_Env}); this:Hide();");
            //DoString("PushEvent('TOGGLE_YUANBAOSHOP');");
        }

        public void DauThai() => OutGhost();


        public void OutGhost()
        {
            if (game.TLBB.PlayerState == 9)
                DoString("Player:SendReliveMessage_OutGhost();");
        }

        public void ReturnCount()
        {
            Win.PostMessage(game.Handle, Global.HookMessage, 55, 105);
        }

        public void CountNil()
        {
            Win.PostMessage(game.Handle, Global.HookMessage, 56, 105);
        }

        public void Relive()
        {
            DoString("Player:SendReliveMessage_Relive();");
        }


        public void Move(string point)
        {

            int x = TDT.ParseInt(point);
            int y = TDT.ParseInt(point.Replace(x + ",", ""));
            Move(x, y);
        }

        public void Move(float x,float y)
        {     
            if (game.TLBB.ON_SCENE_TRANSING || game.IsChangeMap)
            {
                return;
            }
            DoString("AutoRunToTarget(" + (int)Math.Round(x, 0, MidpointRounding.AwayFromZero) + "," + (int)Math.Round(y, 0, MidpointRounding.AwayFromZero) + ")");
        }    

        public void Buy(int index,int num)
        {
            DoString("NpcShop:BulkBuyItem(" + index + "," + num + ",0)");
        }

        public void DeleteMission(string name)
        {
            TOGLE_MISSION();
            Thread.Sleep(1000);
            //    DoString("local cnt = 0; while true do local name, content = DataPool:GetPlayerMission_Memo(cnt); if string.find(name, '" + name.Trim() + "') then if DataPool:GetPlayerMission_Variable(cnt, 0) ~= 1 then DataPool:Mission_Abnegate_Popup(cnt,DataPool:GetPlayerMission_Memo(cnt)); end end cnt = cnt + 1; if cnt == 20 then return end end");
            DoString("local cnt = 0; while true do local name, content = DataPool:GetPlayerMission_Memo(cnt); if string.find(name, '" + name.Trim() + "') then DataPool:Mission_Abnegate_Popup(cnt,DataPool:GetPlayerMission_Memo(cnt)); end cnt = cnt + 1; if cnt == 20 then return end end");
            Thread.Sleep(1000);
            MessageBox_Self_OK_Clicked();
        }

        public void Talk(string channel, string txt)
        {
            DoString("Talk:SendChatMessage('" + channel + "', '" + txt + "');");
        }

        public void QuestFrameMissionComplete()
        {
            DoString("QuestFrameMissionComplete(1);");
        }

        public void QuestFrameMissionContinue()
        {
            DoString("QuestFrameMissionContinue(1);");
        }

        public void TOGLE_MISSION()
        {
            DoString("PushEvent('TOGLE_MISSION');");
        }

        public void CLOSE_MISSION()
        {
            DoString("setmetatable(_G, {__index = QuestLog_Env}); this:Hide();");
        }


        //"PushEvent(\"TOGLE_MISSION\");";

        public int[] AddressDoString = new int[20];

        public void DoString(string lua)
        {
            game.DoStringEx(lua);
        }

        public void PackUp()
        {
            game.DoStringEx("PlayerPackage:PackUpPacket(0); PlayerPackage:PackUpPacket(1);");
            if(game.Address.GameType == 1)
            {
                game.DoStringEx("setmetatable(_G, {__index = Packet_Temporary_Env}); Packet_Temporary_CleanButtonClk(); ");
            }
        }


        public void PetEquipSuitDepart_Buttons_Clicked()
        {
            DoString("setmetatable(_G, {__index = PetEquipSuitDepart_Env}); if this:IsVisible() then PetEquipSuitDepart_Buttons_Clicked() end");
        }

        public void SelectServerQuest_Bn1Click()
        {
            DoString("setmetatable(_G, {__index = LoginSelectServerQuest_Env}); if this:IsVisible() then SelectServerQuest_Bn1Click() end");
        }

        public void MessageBox_Self_OK_Clicked()
        {
            DoString("setmetatable(_G, {__index = MessageBox_Self_Env}); if this:IsVisible() then MessageBox_Self_OK_Clicked(); end");
        }
        //"setmetatable(_G, {__index = MessageBox_Self_Env}); if this:IsVisible() then MessageBox_Self_OK_Clicked(); end"
        public void MessageBox_Self2_OK_Clicked()
        {
            DoString("setmetatable(_G, { __index = MessageBox_Self2_Env}); if this:IsVisible() then MessageBox_Self2_Ok_Clicked(); end");
        }
        public void AcceptBox_OK_Clicked()
        {
            DoString("setmetatable(_G, {__index = AcceptBox_Env}); if this:IsVisible() then AcceptBox_OK_Clicked(); end");
        }

        public void QuestFrameAcceptClicked()
        {
            DoString("QuestFrameAcceptClicked();");
        }

        public void ShowPacket()
        {
            DoString(@"setmetatable(_G, {__index = Packet_Env})
                    this:Show()");
        }
    }
}
