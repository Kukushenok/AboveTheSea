using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Item
{
    /// <summary>
    /// Структура для работы с состоянием руки
    /// </summary>
    public struct TargetArmState
    {
        /// <summary>
        /// Положение руки, к которому рука должна стремиться
        /// </summary>
        public Vector3 targetArmPos;
        /// <summary>
        /// Поворот руки, к которому рука должна стремиться
        /// </summary>
        public Quaternion targetArmRot;
        /// <summary>
        /// При значении false рука будет полностью контролироваться аниматором.
        /// </summary>
        public bool isArmDesired;
    }
    /// <summary>
    /// Структура для указания места и направления предмета при отпускании, а также нового родителя.
    /// Установить нового родителя нужно для того, чтобы карманы персонажа тащили за собой предмет.
    /// </summary>
    public struct ItemDestinationDescription
    {
        public Vector3 position;
        public Vector3 normal;
        public Transform newParent;
        public ItemDestinationDescription(RaycastHit hitInfo)
        {
            position = hitInfo.point;
            normal = hitInfo.normal;
            newParent = null;
        }
        public ItemDestinationDescription(Vector3 position)
        {
            this.position = position;
            normal = Vector3.forward;
            newParent = null;
        }
        public ItemDestinationDescription(Vector3 position, Vector3 normal, Transform newParent = null)
        {
            this.position = position;
            this.normal = normal;
            this.newParent = newParent;
        }
    }
    /// <summary>
    /// Класс предмета
    /// </summary>
    public class ItemBehaviour : MonoBehaviour, IItemInteractResponder
    {
        public delegate void OnItemStatusChanged();
        /// <summary>
        /// Событие при подборе предмета игроком
        /// </summary>
        public event OnItemStatusChanged OnPickedUpEvent;
        /// <summary>
        /// Событие при отпускании предмета игроком
        /// </summary>
        public event OnItemStatusChanged OnDroppedEvent;
        /// <summary>
        /// Информация о предмете. Планировалось использовать в сохранениях для того, чтобы указать
        /// тип предмета, а также просто для указания свойства предмета.
        /// </summary>
        [field: SerializeField] public ItemScriptableObject bindedScriptableObject { get; private set; }
        /// <summary>
        /// Держится ли предмет в руке?
        /// </summary>
        public bool isHolding { get; private set; }
        [SerializeField] private BaseItemHoldProcessor holdProcessor;
        [SerializeField] private ItemLogicProcessor itemLogic;
         
        // Интерфейс предмета. Не расширяйте интерфейс в производных классах!
        /// <summary>
        /// Цикл обновления рук. В нём выставляются позиция рук. Вызывается при Update
        /// </summary>
        /// <param name="data"></param>
        public void UpdateHoldingPos(PlayerHoldingData data) => holdProcessor.UpdateHoldingPos(data);
        /// <summary>
        /// Цикл обновления положения предмета относительно рук. Вызывается при LateUpdate (после обновления позиции рук)
        /// </summary>
        /// <param name="data"></param>
        public void AttractItemToHands(PlayerHoldingData data) => holdProcessor.AttractItemToHands(data);
        /// <summary>
        /// Осуществить процесс подбора предмета.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IEnumerator BeginHoldProcess(PlayerHoldingData data)
        {
            if (isHolding) yield break;

            OnPickedUpEvent?.Invoke();
            yield return holdProcessor.OnBeginHold(data);
            isHolding = true;
        }
        /// <summary>
        /// Осуществить процесс сброса предмета
        /// </summary>
        /// <param name="data"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public IEnumerator EndHoldingProcess(PlayerHoldingData data, ItemDestinationDescription destination)
        {
            if (!isHolding) yield break;

            OnDroppedEvent?.Invoke();
            isHolding = false;
            yield return holdProcessor.OnStopHold(data, destination);
        }

        public virtual bool OnInteracted(PlayerItemManagerScript manager)
        {
            return manager.PickupItem(this);
        }
        public void OnLeftClickInteraction(PlayerCore core, InputAction.CallbackContext context)
        {
            if (!isHolding || !itemLogic) return;
            itemLogic.OnLeftClick(core, context);
        }
        public void OnRightClickInteraction(PlayerCore core, InputAction.CallbackContext context)
        {
            if (!isHolding || !itemLogic) return;
            itemLogic.OnRightClick(core, context);
        }
    }
}