using UnityEngine;

namespace Player
{
    /// <summary>
    /// Абстрактный класс - вид взаимодействия игрока с какими-либо объектами.
    /// </summary>
    public abstract class Interactor
    {
        /// <summary>
        /// Функция обрабатывает или не обрабатывает взаимодействие игрока.
        /// </summary>
        /// <param name="hit">Точка взаимодействия. hit.gameObject для того, чтобы получить объект</param>
        /// <param name="overrideOthers">При установке в true перекрывает все остальные виды взаимодействия</param>
        public abstract void InteractionHit(RaycastHit hit, ref bool overrideOthers);
    }
}