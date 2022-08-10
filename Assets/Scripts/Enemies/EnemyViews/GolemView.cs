﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Enemies.EnemyViews
{
    public class GolemView : EnemyView
    {
        public void OnDieAnimationComplete()
        {
            _enemy.Recycle();
        }
    }
}
