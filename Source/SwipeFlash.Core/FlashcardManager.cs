using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwipeFlash.Core
{
    public class FlashcardManager
    {
        #region Public Properties

        /// <summary>
        /// The maximum length of the card queue
        /// </summary>
        public int CardQueueMaxLength = 10;

        /// <summary>
        /// The queue of card data elements
        /// </summary>
        public List<FlashcardData> CardQueue;

        #endregion


        #region Constructor

        public FlashcardManager()
        {
            UpdateCardQueue();
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Asynchronously updates the card queue
        /// </summary>
        private async void UpdateCardQueue()
        {
            while (CardQueue.Count < CardQueueMaxLength)
            {
                CardQueue.Add(new FlashcardData()
                {
                    Side1Text="The door",
                    Side2Text="La puerta",
                    Side1Icon="Yolo",
                    Side2Icon="Swag",
                    IsInverted=false,
                    HasIllustration=true,
                });
            }

            await Task.Delay(1);
        }

        /// <summary>
        /// Gets the next card in the queue
        /// </summary>
        private FlashcardData GetNext()
        {
            // Get first element
            FlashcardData nextCard = CardQueue[0];

            // Remove element from list
            CardQueue.RemoveAt(0);

            UpdateCardQueue();

            return nextCard;
        }

        #endregion
    }
}
