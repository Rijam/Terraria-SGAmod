﻿using System;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SGAmod
{
	interface IDevItem
	{
		(string, string) DevName();

	}
	interface IRadioactiveItem
	{
		int RadioactiveHeld();
		int RadioactiveInventory();
	}
	interface IShieldItem
	{

	}

	interface IRustBurnText
	{

	}

	interface IDankSlowText
	{

	}
	interface IShieldBashProjectile
	{

	}
	interface IAuroraItem
	{

	}	
	interface IManifestedItem
    {

    }
	interface IJablinItem
	{

	}
	interface ITrueMeleeProjectile
	{

	}
	interface IDrawAdditive
	{
		void DrawAdditive(SpriteBatch spriteBatch);
	}

	interface IPostEffectsDraw
	{
		void PostEffectsDraw(SpriteBatch spriteBatch,float drawScale);
	}
	interface IHitScanItem
	{
	}
	interface ITechItem
	{
		/*int MaxElectricCharge();
		int ElectricChargePerUse();
		int ElectricChargeWhileInUse();*/
	}
	interface IHopperInterface
	{
		bool HopperInputItem(Item item,Point tilelocation,int movementCount, ref bool testOnly);

		bool HopperExportItem(ref Item item, Point tilelocation, int movementCount, ref bool testOnly);
	}	
	interface ISGABoss
	{
		string Trophy();
		bool Chance();
	}

}