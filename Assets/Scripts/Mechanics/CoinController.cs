using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This class animates all token instances in a scene.
    /// This allows a single update call to animate hundreds of sprite 
    /// animations.
    /// If the tokens property is empty, it will automatically find and load 
    /// all token instances in the scene at runtime.
    /// </summary>
    public class CoinController : MonoBehaviour
    {
        [Tooltip("Frames per second at which tokens are animated.")]
        public float frameRate = 12;
        [Tooltip("Instances of tokens which are animated. If empty, token instances are found and loaded at runtime.")]
        public CoinInstance[] coins;

        float nextFrameTime = 0;

        [ContextMenu("Find All Tokens")]
        void FindAllCoinsInScene()
        {
            coins = UnityEngine.Object.FindObjectsOfType<CoinInstance>();
        }

        void Awake()
        {
            //if tokens are empty, find all instances.
            //if tokens are not empty, they've been added at editor time.
            if (coins.Length == 0)
                FindAllCoinsInScene();
            //Register all tokens so they can work with this controller.
            for (var i = 0; i < coins.Length; i++)
            {
                coins[i].tokenIndex = i;
                coins[i].controller = this;
            }
        }

        void Update()
        {
            //if it's time for the next frame...
            if (Time.time - nextFrameTime > (1f / frameRate))
            {
                //update all tokens with the next animation frame.
                for (var i = 0; i < coins.Length; i++)
                {
                    var coin = coins[i];
                    //if token is null, it has been disabled and is no longer animated.
                    if (coin != null)
                    {
                        coin._renderer.sprite = coin.sprites[coin.frame];
                        if (coin.collected && coin.frame == coin.sprites.Length - 1)
                        {
                            coin.gameObject.SetActive(false);
                            coins[i] = null;
                        }
                        else
                        {
                            coin.frame = (coin.frame + 1) % coin.sprites.Length;
                        }
                    }
                }
                //calculate the time of the next frame.
                nextFrameTime += 1f / frameRate;
            }
        }

    }
}