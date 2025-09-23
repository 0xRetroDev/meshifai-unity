using System;
using UnityEngine;

namespace MeshifAI.Core
{
    /// <summary>
    /// Component attached to generated models that contains information about how they were created
    /// </summary>
    public class ModelGenerationData : MonoBehaviour
    {
        /// <summary>
        /// The prompt used to generate the model
        /// </summary>
        public string Prompt { get; private set; }

        /// <summary>
        /// Whether high resolution mode was used during generation
        /// </summary>
        public bool HighRes { get; private set; }

        /// <summary>
        /// When the model was generated
        /// </summary>
        public DateTime GenerationTime { get; private set; }

        /// <summary>
        /// Initialize the generation data
        /// </summary>
        /// <param name="prompt">The text prompt used for generation</param>
        /// <param name="highRes">Whether high resolution mode was used</param>
        public void Initialize(string prompt, bool highRes)
        {
            Prompt = prompt;
            HighRes = highRes;
            GenerationTime = DateTime.Now;
        }

        /// <summary>
        /// Initialize the generation data with a specific time
        /// </summary>
        /// <param name="prompt">The text prompt used for generation</param>
        /// <param name="highRes">Whether high resolution mode was used</param>
        /// <param name="generationTime">When the model was generated</param>
        public void Initialize(string prompt, bool highRes, DateTime generationTime)
        {
            Prompt = prompt;
            HighRes = highRes;
            GenerationTime = generationTime;
        }
    }
}