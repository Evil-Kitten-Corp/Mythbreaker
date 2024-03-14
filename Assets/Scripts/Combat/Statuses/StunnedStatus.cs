using UnityEngine;

namespace Combat.Statuses
{
    public class StunnedStatus: IStatusEffect
    {
        public float duration = 2f;
        public EntityCombat entity;

        private readonly float _endTime;

        public StunnedStatus(float duration, EntityCombat entity)
        {
            this.duration = duration;
            this.entity = entity;
            _endTime = Time.time + duration;
        }

        public Status status => Status.Stun;

        public void Update()
        {
            if (Time.time >= _endTime)
            {
                // Remove the stunned status when the duration expires
                entity.RemoveStatus(Status.Stun);
            }
        }
    }
}