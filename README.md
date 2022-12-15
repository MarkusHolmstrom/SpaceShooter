# SpaceShooter

In this project I used the Unity DOTS for some functionalities. 
For how the bullets/projectiles moved, I used Entities and a job system to track their and the ships positions to ease detection of collision between these objects but not themselves. I cached the game objects transforms to save on performance and a LCG PRNG instead of the random fucntions Unity provides. All game objects that were more than just one of I created a pooling system and deactivated them until they were needed. 
