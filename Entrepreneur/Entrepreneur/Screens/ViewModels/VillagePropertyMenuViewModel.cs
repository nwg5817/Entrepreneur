﻿using Entrepreneur.Classes;
using Entrepreneur.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Entrepreneur.Screens.ViewModels
{
    class VillagePropertyMenuViewModel : ViewModel
    {
		private VillageData _villageData;

		[DataSourceProperty]
		public string VillageDescription
		{
			get
			{
				return "You arrive to the village of " + this._villageData.Settlement.Name + ".";
			}
		}

		[DataSourceProperty]
		public string AcreDescription1
		{
			get
			{
				return $"There are {this._villageData.totalAcres} total acres of land. The villagers are farming {this._villageData.takenAcres} of them for their lord and {this._villageData.playerAcres} for you.";
			}
		}

		[DataSourceProperty]
		public string AcreDescription2
		{
			get
			{
				return $"There are currently {this._villageData.AvailableAcres} acres of land for sale.";
			}
		}

		[DataSourceProperty]
		public string PriceDescription
		{
			get
			{
				return $"One plot of land is worth {this._villageData.PricePerAcre}.";
			}
		}
		[DataSourceProperty]
		public string ProductionDescription
		{
			get
			{
				return $"One plot of land produces {this._villageData.ProductionValue} per day.";
			}
		}
		[DataSourceProperty]
		public string RelationsDescription
		{
			get
			{
				return $"Relation with village = {(int) Math.Round(this._villageData.RelationWithPlayer)}.";
			}
		}
		[DataSourceProperty]
		public string SellMarginDescription
		{
			get
			{
				string SellMargin = $"Sell margin = -{(int)(this._villageData.AcreBuyPercentage * 100)}%";
				return SellMargin;
			}
		}
		[DataSourceProperty]
		public string BuyMarginDescription
		{
			get
			{
				string BuyMargin = $"Buy margin = {(int)(this._villageData.AcreSellPercentage * 100)}%";
				if (this._villageData.AcreSellPercentage > 0) BuyMargin = $"Buy margin = +{(int)(this._villageData.AcreSellPercentage * 100)}%";
				return BuyMargin;
			}
		}

		[DataSourceProperty]
		public string BuyDescription
		{
			get
			{
				return $"{this._villageData.AcreSellPrice}";
			}
		}

		[DataSourceProperty]
		public string SellDescription
		{
			get
			{
				return $"{this._villageData.AcreBuyPrice}";
			}
		}

		[DataSourceProperty]
		public string TotalRevenueDescription
		{
			get
			{
				return $"Total daily player revenue here = {this._villageData.VillagePlayerRevenue}";
			}
		}

		[DataSourceProperty]
		public int PlayerGold
		{
			get
			{
				return Hero.MainHero.Gold;
			}
		}

		[DataSourceProperty]
		public String AvailablePlots
		{
			get
			{
				int availableAcres = this._villageData.totalAcres - (this._villageData.takenAcres + this._villageData.playerAcres);
				return "Available plots: " + availableAcres.ToString();
			}
		}

		[DataSourceProperty]
		public String OwnedPlots
		{
			get
			{
				int ownedPlots = this._villageData.playerAcres;
				return "Owned plots: " + ownedPlots.ToString();
			}
		}

		public VillagePropertyMenuViewModel(ref VillageData acreProperties)
		{
			this._villageData = acreProperties;
		}

		[DataSourceProperty]
		public bool CanBuy
		{
			get
			{
				if (EntrepreneurModel.TotalPlayerPlots < EntrepreneurModel.MaximumPlots)
				{
					return true;
				}
				else return false;
			}
		}
		private void ExitVillagePropertyMenu()
		{
			ScreenManager.PopScreen();
		}
		private void BuyAcre()
		{
			int buyPrice = this._villageData.AcreSellPrice;
			if (this._villageData.AvailableAcres > 0)
			{
				if (Hero.MainHero.Gold >= buyPrice)
				{
					this._villageData.buyAcre();
					GiveGoldAction.ApplyForCharacterToSettlement(Hero.MainHero, Settlement.CurrentSettlement, buyPrice);
					this.RefreshProperties();
				}
				else InformationManager.DisplayMessage(new InformationMessage("You dont have enouph gold to buy this plot."));
			}
			else InformationManager.DisplayMessage(new InformationMessage("There are no plots acres to buy."));
		}
		private void SellAcre()
		{
			int sellPrice = this._villageData.AcreBuyPrice;
			if (this._villageData.playerAcres > 0)
			{
				this._villageData.sellAcre();
				GiveGoldAction.ApplyForSettlementToCharacter(Settlement.CurrentSettlement, Hero.MainHero, sellPrice);
				this.RefreshProperties();
			}
			else InformationManager.DisplayMessage(new InformationMessage("You have no plots to sell."));
		}

		private void RefreshProperties()
		{
			OnPropertyChanged("VillageDescription");
			OnPropertyChanged("AcreDescription1");
			OnPropertyChanged("AcreDescription2");
			OnPropertyChanged("SellDescription");
			OnPropertyChanged("BuyDescription");
			OnPropertyChanged("PriceDescription");
			OnPropertyChanged("ProductionDescription");
			OnPropertyChanged("RelationsDescription");
			OnPropertyChanged("SellMarginDescription");
			OnPropertyChanged("BuyMarginDescription");
			OnPropertyChanged("TotalRevenueDescription");
			OnPropertyChanged("PlayerGold");
			OnPropertyChanged("AvailablePlots");
			OnPropertyChanged("OwnedPlots");
			OnPropertyChanged("CanBuy");
		}
	}
}
