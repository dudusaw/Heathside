using UnityEngine;

namespace Game.Animation
{
    public static class AnimatorArgs

    {
        public static int attackTag { get; }
        public static int isRunning { get; }
        public static int jump { get; }
        public static int onGround { get; }
        public static int slash { get; }
        public static int activateSlash { get; }
        public static int Player_jump { get; }
        public static int Player_fall { get; }
        public static int Player_sword_attack1 { get; }
        public static int Player_sword_attack2 { get; }
        public static int Player_sword_attack3 { get; }
        public static int Player_dash_start { get; }
        public static int Player_dash_end { get; }
        public static int Player_bow_stand { get; }
        public static int Player_idle { get; }
        public static int Player_empty_attack_1 { get; }
        public static int Player_empty_attack_2 { get; }
        public static int Player_empty_attack_3 { get; }

        static AnimatorArgs()
        {
            isRunning = Animator.StringToHash("isRunning");
            onGround = Animator.StringToHash("onGround");
            slash = Animator.StringToHash("Simple_slash");
            activateSlash = Animator.StringToHash("activateSlash");
            attackTag = Animator.StringToHash("attack");
            Player_jump = Animator.StringToHash("Player_jump");
            Player_fall = Animator.StringToHash("Player_fall");
            Player_sword_attack1 = Animator.StringToHash("Player_sword_attack1");
            Player_sword_attack2 = Animator.StringToHash("Player_sword_attack2");
            Player_sword_attack3 = Animator.StringToHash("Player_sword_attack3");
            Player_dash_start = Animator.StringToHash("Player_dash_start");
            Player_dash_end = Animator.StringToHash("Player_dash_end");
            Player_bow_stand = Animator.StringToHash("Player_bow_stand");
            Player_idle = Animator.StringToHash("Player_idle");
            Player_empty_attack_1 = Animator.StringToHash("Player_empty_attack_1");
            Player_empty_attack_2 = Animator.StringToHash("Player_empty_attack_2");
            Player_empty_attack_3 = Animator.StringToHash("Player_empty_attack_3");
        }
    }
}