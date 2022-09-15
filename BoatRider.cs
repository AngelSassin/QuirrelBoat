using System;
using UnityEngine;

namespace QuirrelBoat
{
    internal class BoatRider : MonoBehaviour
    {
        internal bool riding = false;
        internal Boat boatObject = null;

        public void Awake()
        {
            riding = false;
        }

        public void Update()
        {
            if (riding)
            {
                HeroController.instance.gameObject.GetComponent<tk2dSpriteAnimator>().enabled = false;
                HeroController.instance.gameObject.GetComponent<tk2dSpriteAnimator>().Play("Sit Fall Asleep");
                HeroController.instance.cState.onGround = true;
                HeroController.instance.cState.falling = false;
                HeroController.instance.cState.willHardLand = false;
                if (InputHandler.Instance.inputActions.jump.IsPressed)
                {
                    StopRiding();
                    return;
                }
                setPosition();
            }
        }
        public void FixedUpdate()
        {
            if (!QuirrelBoat.Instance.quirrel_prefab.activeSelf)
                return;

            if (shouldBeginRiding())
            {
                riding = true;
                boatObject = QuirrelBoat.Instance.quirrel_prefab.GetComponent<Boat>();
                HeroController.instance.cState.onGround = true;
                HeroController.instance.cState.falling = false;
                HeroController.instance.cState.willHardLand = false;
                boatObject.rider = this;
            }
        }

        public bool shouldBeginRiding()
        {
            if (riding)
                return false;

            if (!HeroController.instance.cState.falling)
                return false;

            if (HeroController.instance.gameObject.transform.position.y >= 23.2F)
                return false;

            Vector3 boatDistance = HeroController.instance.transform.position - QuirrelBoat.Instance.quirrel_prefab.transform.position;
            if (boatDistance.magnitude > 1.65F)
                return false;

            return true;
        }

        internal void StopRiding()
        {
            HeroController.instance.gameObject.GetComponent<tk2dSpriteAnimator>().enabled = true;
            riding = false;
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            boatObject.rider = null;
            HeroController.instance.cState.onGround = false;
            HeroController.instance.cState.falling = true;
        }

        private void setPosition()
        {
            gameObject.transform.position = new Vector3(221.1F, 23.70F, 0.004F)
                + new Vector3((float)Math.Sin(boatObject.offset / 112.0F) / 6, (float)Math.Cos(boatObject.offset / 74.0F) / 10, 0)
                + new Vector3(boatObject.lakePosition, 0, 0)
                + new Vector3((float)(Math.Sin(boatObject.offset / 52.0F) * -0.19), 0, 0)
                + new Vector3(boatObject.lift/-110.0F, 0, 0);
            gameObject.transform.SetRotation2D((float)(Math.Sin(boatObject.offset / 52.0F) * 8) + (boatObject.lift/1.50F));
        }
    }
}