using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace MPH
{
    public class Building_MPH : Building_Grave
    {
        private CompMPHStorage storageComp;
        public CompMPHStorage StorageComp
        {
            get
            {
                if (storageComp == null)
                    storageComp = GetComp<CompMPHStorage>();
                return storageComp;
            }
        }

        public void TransferBuriedCorpses()
        {
            if (StorageComp == null || innerContainer == null || innerContainer.Count == 0)
                return;

            // 先复制一份，避免遍历时修改集合
            var toTransfer = new List<Thing>();
            foreach (Thing t in innerContainer)
            {
                if (t is Corpse)
                    toTransfer.Add(t);
            }

            foreach (Thing t in toTransfer)
            {
                innerContainer.Remove(t);
                StorageComp.AddCorpse((Corpse)t);
            }
        }
    }
}