using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace MPH
{
    public class CompMPHStorage : ThingComp
    {
        // 核心：动态列表，没有硬性上限
        public List<Corpse> storedCorpses = new List<Corpse>();

        // 必须实现：存档时保存列表，读档时恢复
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Collections.Look(ref storedCorpses, "storedCorpses", LookMode.Deep);

            // 防御性代码：确保读档后列表不为 null
            if (Scribe.mode == LoadSaveMode.PostLoadInit && storedCorpses == null)
            {
                storedCorpses = new List<Corpse>();
            }
        }

        public void AddCorpse(Corpse corpse)
        {
            if (corpse != null && !storedCorpses.Contains(corpse))
            {
                storedCorpses.Add(corpse);
            }
        }

        public bool RemoveCorpse(Corpse corpse)
        {
            return storedCorpses.Remove(corpse);
        }

        public override void CompTickRare()
        {
            base.CompTickRare();
            (parent as Building_MPH)?.TransferBuriedCorpses();
        }
    }
}
