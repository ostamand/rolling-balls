using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public enum TypeOf { Player, Agent };


    public class Helper
    {
        public static bool CheckFellOff(Vector3 position)
        {
            bool off = false;
            if (position.y < -1.0f || position.y > 1.0f)
            {
                off = true;
            }
            return off;
        }

    }
}
