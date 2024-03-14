using UnityEngine;

namespace Combat.Statuses
{
    public class IgnitedStatus: IStatusEffect
    {
        public float tickDamage = 5f; // Damage per tick
        public float tickInterval = 1f; // Interval between each tick
        public float duration = 10f; // Duration of the status effect

        public EntityCombat entity;

        private float nextTickTime;
        private float endTime;

        public IgnitedStatus(float tickDamage, float tickInterval, float duration, EntityCombat entity)
        {
            this.tickDamage = tickDamage;
            this.tickInterval = tickInterval;
            this.duration = duration;
            this.entity = entity;
            nextTickTime = Time.time + tickInterval;
            endTime = Time.time + duration;
        }
        
        public Status status => Status.Ignite;

        public void Update()
        {
            if (Time.time >= nextTickTime)
            {
                entity.TakeDamage(tickDamage);
                nextTickTime = Time.time + tickInterval;
            }

            if (Time.time >= endTime)
            {
                entity.RemoveStatus(Status.Ignite);
            }
        }
    }
}