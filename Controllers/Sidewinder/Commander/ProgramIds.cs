using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Sidewinder.Commander
{
    public class ProgramIds
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public class ProgramItem
        {
            public string ControllerType { get; set; }
            public bool Locked { get; set; }
            public uint LockedButton { get; set; }
            public SwscLight Light { get; set; }
            public uint CurrentId { get; set; }
            public bool Shifted { get; set; }
        }

        public SwscController Controller { get; set; }
        public ScController ScController { get; set; }

        public Dictionary<uint, ProgramItem> Items { get; set; }

        public void ButtonPressed(uint button)
        {
            RemapButton(button);
            CheckLocked(button);
        }

        public void RemapButton(uint button)
        {
            if (false == Items.ContainsKey(button)) { return; }

            var item = Items[button];
            if (item.Locked)
                return;

            var availableItems = Items.Values.Where(x => x.Locked == false);

            uint currentJoyId = ScController.vJoyMapper.vJoyMap[item.ControllerType];
            uint selectedId = uint.MaxValue;
            ProgramItem selectedItem = null;
            
            foreach (var available in availableItems)
            {
                uint availableId = ScController.vJoyMapper.vJoyMap[available.ControllerType];
                if (availableId > currentJoyId && availableId < selectedId)
                {
                    selectedId = availableId;
                    selectedItem = available;
                }
            }

            if (selectedItem == null)
            {
                selectedId = uint.MaxValue;
                foreach (var available in availableItems)
                {
                    uint availableId = ScController.vJoyMapper.vJoyMap[available.ControllerType];
                    if (availableId < selectedId)
                    {
                        selectedId = availableId;
                        selectedItem = available;
                    }
                }
            }

            if (selectedItem != null)
            {
                log.Debug($"Swapping {item.ControllerType} with {selectedItem.ControllerType}");
                ScController.vJoyMapper.vJoyMap[item.ControllerType] = ScController.vJoyMapper.vJoyMap[selectedItem.ControllerType];
                ScController.vJoyMapper.vJoyMap[selectedItem.ControllerType] = currentJoyId;
            }
        }

        public void CheckLocked(uint button)
        {
            foreach(var item in Items.Values)
            {
                if (item.LockedButton == button)
                {
                    item.Locked = !item.Locked;
                    UpdateLockedLights((button & (uint)SwscButton.Shift1) != 0);
                }
            }
        }

        public void UpdateLockedLights(bool shifted)
        {
            var lights = new List<SwscLight>();
            foreach (var item in Items.Values)
            {
                if (item.Locked && shifted == item.Shifted)
                    lights.Add(item.Light);
            }
            Controller.SetLights(lights.ToArray());
        }

        public ProgramIds()
        {
            Items = new Dictionary<uint, ProgramItem>
            {
                {
                    (uint)1,
                    new ProgramItem
                    {
                        ControllerType = vJoyTypes.StickAndPedals,
                        Locked = false,
                        LockedButton = 4,
                        Light = SwscLight.Button4,
                        Shifted = false
                    }
                },
                {
                    (uint)2,
                    new ProgramItem
                    {
                        ControllerType = vJoyTypes.Throttle,
                        Locked = false,
                        LockedButton = 5,
                        Light = SwscLight.Button5,
                        Shifted = false
                    }
                },
                {
                    (uint)3,
                    new ProgramItem
                    {
                        ControllerType = vJoyTypes.Commander,
                        Locked = false,
                        LockedButton = 6,
                        Light = SwscLight.Button6,
                        Shifted = false
                    }
                },
                {
                    (uint)((uint)SwscButton.Shift1 | 1),
                    new ProgramItem
                    {
                        ControllerType = vJoyTypes.Buttons,
                        Locked = false,
                        LockedButton = ((uint)SwscButton.Shift1 | 4),
                        Light = SwscLight.Button4,
                        Shifted = true
                    }
                },
                {
                    (uint)((uint)SwscButton.Shift1 | 2),
                    new ProgramItem
                    {
                        ControllerType = vJoyTypes.Voice,
                        Locked = false,
                        LockedButton = ((uint)SwscButton.Shift1 | 5),
                        Light = SwscLight.Button5,
                        Shifted = true
                    }
                },
                {
                    (uint)((uint)SwscButton.Shift1 | 3),
                    new ProgramItem
                    {
                        ControllerType = vJoyTypes.Virtual,
                        Locked = false,
                        LockedButton = ((uint)SwscButton.Shift1 | 6),
                        Light = SwscLight.Button6,
                        Shifted = true
                    }
                },
            };
        }
    }
}
