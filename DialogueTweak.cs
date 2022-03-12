global using DialogueTweak.Interfaces;
global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using ReLogic.Content;
global using ReLogic.Graphics;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using Terraria;
global using Terraria.Audio;
global using Terraria.GameContent;
global using Terraria.GameContent.UI.States;
global using Terraria.GameInput;
global using Terraria.ID;
global using Terraria.Localization;
global using Terraria.ModLoader;
global using Terraria.UI;
global using Terraria.UI.Chat;
global using Terraria.UI.Gamepad;
// C# 10.0�¼ӵ�global using https://zhuanlan.zhihu.com/p/433239269

namespace DialogueTweak
{
    public partial class DialogueTweak : Mod
    {
		internal static DialogueTweak instance;
		internal static GUIChatDraw MobileChat;

		public override void Load() {
			instance = this;
			On.Terraria.Main.GUIChatDrawInner += Main_GUIChatDrawInner;
		}

		public override void Unload() {
			instance = null;
        }

		// ͨ������screenWidthʹһ�л��Ƶ���Ļ֮�⣬NPC�Ի����Ʋ��ᱻӰ��
		private void Main_GUIChatDrawInner(On.Terraria.Main.orig_GUIChatDrawInner orig, Main self) {
			// ȷ���Ǵ���NPC�Ի�״̬��PC���б༭��ʾ��ʲô��Ҳ�����UI��
			GUIChatDraw.GUIDrawInner();
		}
	}
}