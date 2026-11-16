using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace MPH
{
    public class ITab_MPH : ITab
    {
        private Vector2 scrollPosition = Vector2.zero;

        // 只在选中你的坟墓时显示
        public override bool IsVisible => SelThing is Building_MPH;

        public ITab_MPH()
        {
            labelKey = "MPH_MultiGraveTab";
            size = new Vector2(400f, 500f);
        }

        protected override void FillTab()
        {
            var grave = SelThing as Building_MPH;
            if (grave?.StorageComp == null) return;

            var corpses = grave.StorageComp.storedCorpses;

            // 内容区域高度随尸体数量动态增长
            Rect viewRect = new Rect(0f, 0f, size.x - 20f, corpses.Count * 35f);
            //y 从 0f 改为 24f，高度减少 24f，整体下移，避开"X"
            Rect scrollOutRect = new Rect(0f, 24f, size.x, size.y - 24f);
            Widgets.BeginScrollView(scrollOutRect, ref scrollPosition, viewRect);

            for (int i = 0; i < corpses.Count; i++)
            {
                Corpse corpse = corpses[i];
                Rect rowRect = new Rect(0f, i * 35f, viewRect.width, 35f);

                // 尸体图标和名称
                Widgets.ThingIcon(new Rect(rowRect.x, rowRect.y + 2f, 30f, 30f), corpse);
                Widgets.Label(new Rect(rowRect.x + 35f, rowRect.y, rowRect.width - 100f, 35f), corpse.LabelCap);

                // “取出”按钮
                if (Widgets.ButtonText(new Rect(rowRect.xMax - 60f, rowRect.y, 60f, 30f), "MPH_TakeOut".Translate()))
                {
                    grave.StorageComp.RemoveCorpse(corpse);
                    GenPlace.TryPlaceThing(corpse, grave.Position, grave.Map, ThingPlaceMode.Near);
                    break; // 列表已修改，跳出循环，下一帧刷新
                }
            }

            Widgets.EndScrollView();
        }
    }
}