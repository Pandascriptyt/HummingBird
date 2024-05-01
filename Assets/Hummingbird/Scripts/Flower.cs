using UnityEngine;

namespace Hummingbird.Scripts
{
    /// <summary>
    /// Manages a single flower with nectar
    /// </summary>
    public class Flower : MonoBehaviour
    {
        [Tooltip("The color when the flower is full")]
        public Color fullFlowerColor = new Color(1f, 0f, .3f);

        [Tooltip("The color when the flower is empty")]
        public Color emptyFlowerColor = new Color(.5f, 0f, 1f);

        /// <summary>
        /// The trigger collider representing the nectar
        /// </summary>
        [HideInInspector]
        public Collider nectarCollider;

        // The solid collider representing the flower petals
        private Collider flowerCollider;

        // The flower's material
        private Material flowerMaterial;

        /// <summary>
        /// A vector pointing straight out of the flower
        /// </summary>
        public Vector3 FlowerUpVector
        {
            get
            {
                return nectarCollider.transform.up;
            }
        }

        /// <summary>
        /// The center position of the nectar collider
        /// </summary>
        public Vector3 FlowerCenterPosition
        {
            get
            {
                return nectarCollider.transform.position;
            }
        }

        /// <summary>
        /// The amount of nectar remaining in the flower
        /// </summary>
        public float NectarAmount { get; private set; }

        /// <summary>
        /// Whether the flower has any nectar remaining
        /// </summary>
        public bool HasNectar
        {
            get
            {
                return NectarAmount > 0f;
            }
        }
    
    

        private float ExtractNectarFromFlower(float extractionAmount)
        {
            float nectarTaken = Mathf.Clamp(extractionAmount, 0f, NectarAmount);
        
        
            // Subtract the nectar
            NectarAmount -= extractionAmount;

            if (NectarAmount <= 0)
            {
                // No nectar remaining
                NectarAmount = 0;

                // Disable the flower and nectar colliders
                DeactivateFlower();
            }
            return nectarTaken;
        }

        private void DeactivateFlower()
        {
            FlowerIsActive(false);
        }
        private void ReactivateFlower()
        {
            FlowerIsActive(true);
        }

        private void FlowerIsActive(bool flowerstate)
        {
            flowerCollider.gameObject.SetActive(flowerstate);
            nectarCollider.gameObject.SetActive(flowerstate);

            // Change the flower color to indicate that it is empty
            if (flowerstate)
            {
                flowerMaterial.SetColor("_BaseColor", fullFlowerColor);
            }
            else
            {
                flowerMaterial.SetColor("_BaseColor", emptyFlowerColor);  
            }
        
        }

        /// <summary>
        /// Attempts to remove nectar from the flower
        /// </summary>
        /// <param name="amount">The amount of nectar to remove</param>
        /// <returns>The actual amount successfully removed</returns>
        public float Feed(float amount)
        {
            // Return the amount of nectar that was taken
            return ExtractNectarFromFlower(amount);
        }
    
   
        public void ResetFlower()
        {
            // Refill the nectar
            NectarAmount = 1f;

            ReactivateFlower();
        }

    

        /// <summary>
        /// Called when the flower wakes up
        /// </summary>
        private void Awake()
        {
            // Find the flower's mesh renderer and get the main material
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            flowerMaterial = meshRenderer.material;

            // Find flower and nectar colliders
            flowerCollider = transform.Find("FlowerCollider").GetComponent<Collider>();
            nectarCollider = transform.Find("FlowerNectarCollider").GetComponent<Collider>();
        }
    }
}
