using UnityEngine;

namespace HealthDrink.Behavior
{
    internal class HealPlayer : PhysicsProp
    {
        private readonly string[] allLines = new string[1] { "Consume Healing Banana: [RMB]" };

        public override void SetControlTipsForItem()
        {
            base.SetControlTipsForItem();
            if (base.IsOwner && playerHeldBy != null)
            {
                HUDManager.Instance.ChangeControlTipMultiple(allLines, holdingItem: true, itemProperties);
                Plugin.Logger.LogDebug("UI THING");
            }
            else
            {
                HUDManager.Instance.ClearControlTips();
            }
        }

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);

            Plugin.Logger.LogDebug($"Health: {playerHeldBy.health}");
            if (buttonDown && playerHeldBy.health < 100) // some weird vodo.
            {
                if (playerHeldBy.health < 100)
                {
                    int newHealth = Mathf.Min(playerHeldBy.health + 10, 100);
                    playerHeldBy.health = newHealth;
                    Plugin.Logger.LogDebug($"Health (Healing): {playerHeldBy.health}");

                    if (playerHeldBy.health >= 100) // What am I doing?
                    {
                        HUDManager.Instance.selfRedCanvasGroup.alpha = 0f;
                    }
                }

                HUDManager.Instance.flashFilter = 7f / (5f * 10f); // Flash effect thing when healing.
                Destroy(gameObject);
                playerHeldBy.DiscardHeldObject();
                
            }
            else
            {
                Plugin.Logger.LogDebug($"Health (Full): {playerHeldBy.health}");
            }

        }

    }
}
