using Aster.Entity.Enemy;
using QFSW.QC;
using Sirenix.Utilities;
using UnityEngine;

namespace Aster.Utils
{
    public static class ConsoleCommands
    {
        [Command(aliasOverride: "kill-all")]
        public static void KillAllEnemies()
        {
            GameObject.FindGameObjectsWithTag("Enemy").ForEach((go) => go.GetComponent<EnemyController>().HP.Set(0));
        }
    }
}