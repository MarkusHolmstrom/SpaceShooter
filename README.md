# SpaceShooter

In this project I used the Unity DOTS for some functionalities. 
For how the bullets/projectiles moved, I used Entities and a job system to track their and the ships positions to ease detection of collision between these objects but not themselves. I cached the game objects transforms to save on performance and used a LCG PRNG that I created earlier instead of the random functions Unity provides. I used a pooling system for all the multiple game objects. 
