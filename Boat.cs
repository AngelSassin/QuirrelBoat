using System;
using UnityEngine;

namespace QuirrelBoat
{
    internal class Boat : MonoBehaviour
    {
        internal float lakePosition = 0;
        internal int offset = 0;
        internal float lift = 0;
        internal float timeMoved = 0;
        internal BoatRider rider = null;

        public void Awake()
        {
            gameObject.transform.position = new Vector3(221.21F, 22.45F, 0.006F);
            gameObject.transform.localPosition = new Vector3(221.5F, 22.51F, 0.006F);
            this.gameObject.transform.localScale = new Vector3(-1.8F, 1.8F, 1);
        }

        public void Update()
        {
            setPosition();

            GameObject splash = GameObject.Find("Splash Pt");
            if (rider != null)
            {
                if (GameManager.instance.isPaused)
                    return;
                if (InputHandler.Instance.inputActions.left.IsPressed && !InputHandler.Instance.inputActions.right.IsPressed)
                {
                    if (this.gameObject.transform.position.x > 27.10F)
                    {
                        float frameSpeed = (Time.time - timeMoved);
                        lift += (-25 - lift) / 100.0F;
                        lakePosition -= frameSpeed < 0.05 ? frameSpeed * 15 : 0.1F;
                        timeMoved = Time.time;
                        splash.transform.position = new Vector3(gameObject.transform.position.x, 22.59F, 1F);
                        if (!splash.GetComponent<ParticleSystem>().isPlaying)
                            splash.GetComponent<ParticleSystem>().Play();
                        splash.transform.SetRotation2D(0);
                    }
                    else splash.GetComponent<ParticleSystem>().Stop();
                    this.gameObject.transform.localScale = new Vector3(-1.8F, 1.8F, 1);
                }
                else if (InputHandler.Instance.inputActions.right.IsPressed && !InputHandler.Instance.inputActions.left.IsPressed)
                {
                    if (this.gameObject.transform.position.x < 223.50F)
                    {
                        float frameSpeed = (Time.time - timeMoved);
                        lift += (30 - lift) / 100.0F;
                        lakePosition += frameSpeed < 0.05 ? frameSpeed * 15 : 0.1F;
                        timeMoved = Time.time;
                        splash.transform.position = new Vector3(gameObject.transform.position.x, 22.59F, 1F);

                        if (!splash.GetComponent<ParticleSystem>().isPlaying)
                            splash.GetComponent<ParticleSystem>().Play();
                        splash.transform.SetRotation2D(124.0873F);
                    }
                    else splash.GetComponent<ParticleSystem>().Stop();
                    this.gameObject.transform.localScale = new Vector3(1.8F, 1.8F, 1);
                }
                else
                {
                    splash.GetComponent<ParticleSystem>().Stop();
                    lift *= 0.99F;
                }
            }
            else
            {
                splash.GetComponent<ParticleSystem>().Stop();
                lift *= 0.99F;
            }
        }

        public void FixedUpdate()
        {
            offset += 1;
        }

        private void setPosition()
        {
            gameObject.transform.position = new Vector3(221.08F, 22.55F, 0.006F)
                + new Vector3((float)Math.Sin(offset / 112.0F) / 6, (float)Math.Cos(offset / 74.0F) / 10, 0)
                + new Vector3(lakePosition, 0, 0)
                + new Vector3(gameObject.transform.localScale.x > 0 ? 0.0F: 0, 0, 0);
            gameObject.transform.SetRotation2D(-5 + (float)(Math.Sin(offset / 52.0F) * 8) + lift);
        }
    }
}