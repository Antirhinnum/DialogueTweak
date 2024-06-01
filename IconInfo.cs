using System;
using System.Collections.Generic;
using DialogueTweak.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DialogueTweak;

internal enum IconType
{
    Happiness,
    Back,
    Shop,
    Extra
}

internal class IconInfo
{
    private static readonly List<string> SpecialIconNames = [
        "Head",
        "QuestFish"
    ];

    public bool IsSpecialIcon => SpecialIconNames.Contains(Texture);

    private readonly Func<string> _textureInternal;
    internal readonly IconType IconType;
    internal readonly List<int> NPCTypes;
    internal string Texture => _textureInternal() ?? "";
    internal Func<bool> Available;
    internal Func<Rectangle> Frame;
    internal Func<float> CustomOffset;

    internal IconInfo(IconType iconType, int npcType, string texture) : this(iconType, [npcType], texture) {
    }

    internal IconInfo(IconType iconType, List<int> npcTypes, string texture) : this(iconType, npcTypes, () => texture) {
    }

    internal IconInfo(IconType iconType, List<int> npcTypes, Func<string> texture) {
        IconType = iconType;
        NPCTypes = npcTypes ?? [NPCID.None];
        _textureInternal = texture;
        if (!Main.dedServ && !ModContent.HasAsset(Texture) && Texture != "" && !IsSpecialIcon) {
            DialogueTweak.Instance.Logger.Warn($"Texture path {Texture} is missing.");
        }

        Available = () => true;
        Frame = null;
        CustomOffset = null;
    }

    public void GetIconParameters(out Func<float> shopCustomOffset, out Asset<Texture2D> texture,
        out Rectangle? texFrameOverride, int head) {
        texFrameOverride = null;
        shopCustomOffset = CustomOffset;
        switch (Texture) {
            case "Head":
                texture = ChatMethods.GetHeadOrDefaultIcon(head);
                break;
            // 有任务鱼未完成时显示任务鱼，否则显示NPC头像
            case "QuestFish":
                if (!Main.anglerQuestFinished && Main.anglerQuestItemNetIDs.IndexInRange(Main.anglerQuest)) {
                    int fishId = Main.anglerQuestItemNetIDs[Main.anglerQuest];
                    if (TextureAssets.Item.IndexInRange(fishId)) {
                        texture = TextureAssets.Item[fishId];
                        break;
                    }
                }

                // 没有完成任务或者任务物品不存在
                texture = ChatMethods.GetHeadOrDefaultIcon(head);
                break;
            default:
                texture = ModContent.Request<Texture2D>(Texture);
                texFrameOverride = Frame?.Invoke();
                break;
        }
    }
}