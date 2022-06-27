using UnityEngine;

public class FoodDishesController : MonoBehaviour {
  /***** Private Variables *****/
  private FoodDishController[] _foodDishControllers;
  
  /***** Unity Methods *****/
  void Start() {
    // Find all children food dishes
    _foodDishControllers = GetComponentsInChildren<FoodDishController>();
  }

  void Update() {
    // Find the food dish fullness percentage.
    float foodDishPercentage = Model.Instance.gameData.foodDishPercentage;
    
    // Given that the foodDishPercentage is a value between 0 and 1,
    // we can use it to find the index of the food dish that is full.
    int maxFoodDishSpriteIndex = _foodDishControllers[0].foodDishSprites.Length - 1;
    int foodDishSpriteIndex = (int) Mathf.Floor(foodDishPercentage * (maxFoodDishSpriteIndex));
    
    // Iterate over each child food dish and update it's sprite image
    foreach (FoodDishController foodDishController in _foodDishControllers) {
      foodDishController.foodDishSpriteIndex = foodDishSpriteIndex;
    }
  }

}
