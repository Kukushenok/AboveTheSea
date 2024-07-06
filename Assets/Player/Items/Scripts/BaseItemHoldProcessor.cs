using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Item
{
    public abstract class BaseItemHoldProcessor : MonoBehaviour
    {
        /// <summary>
        /// Корутина, отвечающая за процесс подбирания предмета (анимация подбора предмета)
        /// </summary>
        /// <param name="data"></param>
        public abstract IEnumerator OnBeginHold(PlayerHoldingData data);
        /// <summary>
        /// Корутина, отвечающая за процесс сброса предмета. Тут уже указывается, куда предмет выявляется.
        /// </summary>
        /// <param name="data"></param>
        public abstract IEnumerator OnStopHold(PlayerHoldingData data, ItemDestinationDescription destination);
        /// <summary>
        /// Цикл обновления рук. В нём выставляются позиция рук. Вызывается при Update
        /// </summary>
        /// <param name="data"></param>
        public abstract void UpdateHoldingPos(PlayerHoldingData data);
        /// <summary>
        /// Цикл обновления положения предмета относительно рук. Вызывается при LateUpdate (после обновления позиции рук)
        /// </summary>
        /// <param name="data"></param>
        public abstract void AttractItemToHands(PlayerHoldingData data);
    }
}