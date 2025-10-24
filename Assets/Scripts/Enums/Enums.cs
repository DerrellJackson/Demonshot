
public enum InventoryLocation
{
  player, 
  chest,
  count
}

public enum PlayerMovement
{
  left,
  right,
  down,
  up,
  idleLeft,
  idleRight,
  idleUp,
  idleDown,
  leftDash,
  rightDash,
  upDash,
  downDash,
  digLeft,
  digRight,
  digUp,
  digDown,
  plantLeft,
  plantRight,
  plantUp,
  plantDown,
  waterLeft,
  waterRight,
  waterUp,
  waterDowm,
  swimUp,
  swimDown,
  swimRight,
  swimLeft,
  carryLeft,
  carryRight,
  carryUp,
  carryDown,
  shakeHeadYes,
  shakeHeadNo,
  surrender,
  cry,
  vanish,
  none
}

public enum PartVariantColour 
{

  none, 
  count

}


public enum PartVariantType 
{

  none,
  carry,
  hoe,
  pickaxe,
  axe,
  scythe,
  wateringCan,
  count

}


public enum GridBoolProperty 
{

  diggable,
  canDropItem,
  canPlaceFurniture,
  isPath,
  isNPCObstacle

}


public enum SceneName
{

  SpawnScene,
  FieldScene,
  CabinScene,
  DungeonScene,
  TownScene,
  RoadTownScene

}


public enum Season 
{
  
  Seeds, //Spring 
  Burning, //Summer
  Unfeeling, //Autumn
  Cold, //Winter
  none,
  count

}


public enum ToolEffect 
{
  none, 
  watering
}

public enum ItemType 
{

  Seed,
  Egg,
  Ore, 
  Food,
  Raw_food,
  Watering_tool,
  Hoeing_tool, 
  Chopping_tool,
  Breaking_tool, 
  Reaping_tool,
  Collecting_tool,
  Fishing_tool,
  Reapable_scenary,
  Furniture,
  Decor,
  Craftable,
  none,
  Melee_weapon,
  Projectile_weapon,
  Explosive_weapon,
  Potion, 
  Drink,
  Honey_tool,
  Flower,
  Raw_Ore,
  Multi_Tool,
  count 

}

public enum Orientation
{
  north,
  east,
  south,
  west,
  none
}

public enum AimDirection
{
  Up, 
  UpRight,
  UpLeft,
  Right,
  Left,
  Down
}

public enum ChestSpawnEvent 
{
  onRoomEntry,
  onEnemiesDefeated
}

public enum ChestSpawnPosition
{
  atSpawnerPosition,
  atPlayerPosition
}

public enum ChestState
{
  closed, 
  healthItem,//will prob take out health/ammo
  ammoItem,
  weaponItem,
  empty //make vanish, where to add state at, may also add a state that adds bad things in chest, look in Chest script for States
}

public enum BushDrops
{
  unused,
  stickItem,
  leafItem,
  rareItem,
  herbItem,
  enemyPopOut,
  empty
}

public enum GameState
{
  gameStarted,
  playingLevel,
  engagingEnemies,
  bossStage,
  engagingBoss,
  levelCompleted,
  gameWon,
  gameLost,
  gamePaused,
  dungeonOverviewMap,
  restartGame
}