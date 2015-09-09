using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using log4net;
using Microbrewit.Api.Service.Util;
using Microbrewit.Model;
using Microbrewit.Model.BeerXml;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Service.Elasticsearch.Component;
using Microbrewit.Service.Elasticsearch.Interface;
using Fermentable = Microbrewit.Model.BeerXml.Fermentable;
using Hop = Microbrewit.Model.BeerXml.Hop;
using MashStep = Microbrewit.Model.BeerXml.MashStep;
using Recipe = Microbrewit.Model.BeerXml.Recipe;
using Yeast = Microbrewit.Model.BeerXml.Yeast;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class BeerXmlResolver : ValueResolver<Recipe, RecipeDto>
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IFermentableElasticsearch _fermentableElasticsearch = new FermentableElasticsearch();
        private readonly IHopElasticsearch _hopElasticsearch = new HopElasticsearch();
        private readonly IHopRepository _hopRepository = new HopDapperRepository();
        private readonly IOtherElasticsearch _otherElasticsearch = new OtherElasticsearch();
        private readonly IYeastElasticsearch _yeastElasticsearch = new YeastElasticsearch();

        protected override RecipeDto ResolveCore(Recipe source)
        {
            var boilSize = (int) double.Parse(source.BoilSize, CultureInfo.InvariantCulture);
            var batchSize = (int)double.Parse(source.BatchSize, CultureInfo.InvariantCulture);

            var recipeDto = new RecipeDto
            {
                MashSteps = new List<MashStepDto>(),
                BoilSteps = new List<BoilStepDto>(),
                FermentationSteps = new List<FermentationStepDto>(),
                SpargeStep = new SpargeStepDto(),
                Notes = source.Taste_Notes,
                //Sets 60min as standard.
                TotalBoilTime = (source.Boil_Time != null) ? double.Parse(source.Boil_Time, CultureInfo.InvariantCulture) : 60,
                Volume = (batchSize <= 0) ? boilSize : batchSize,
            };

            // <PRIMARY_AGE>
            if (source.PrimaryAge != null)
            {
                var primaryAge = (int)double.Parse(source.PrimaryAge, CultureInfo.InvariantCulture);
                var primaryTemp = (source.Primary_Temp == null) ? 0 : (int)double.Parse(source.Primary_Temp, CultureInfo.InvariantCulture);
                var fermentationStep = GetFermentationStepDto(recipeDto, primaryAge);
                fermentationStep.Temperature = primaryTemp;
                recipeDto.FermentationSteps.Add(fermentationStep);
            }

            //<SECONDAY_AGE>
            if (source.Secondary_Age != null)
            {
                var secondaryAge = (int)double.Parse(source.Secondary_Age, CultureInfo.InvariantCulture);
                var primaryTemp = (source.Secondary_Temp == null) ? 0 : (int)double.Parse(source.Secondary_Temp, CultureInfo.InvariantCulture);
                var fermentationStep = GetFermentationStepDto(recipeDto, secondaryAge);
                fermentationStep.Temperature = primaryTemp;
                recipeDto.FermentationSteps.Add(fermentationStep);
            }
            // <TERTIARYAGE>
            if (source.TertiaryAge != null)
            {
                var tertiaryAge = (int)double.Parse(source.TertiaryAge, CultureInfo.InvariantCulture);
                var tertiaryTemp = (source.TertiaryTemp == null) ? 0 : (int)double.Parse(source.TertiaryTemp, CultureInfo.InvariantCulture);
                var fermentationStep = GetFermentationStepDto(recipeDto, tertiaryAge);
                fermentationStep.Temperature = tertiaryTemp;
                recipeDto.FermentationSteps.Add(fermentationStep);
            }


            //Mash step from <MASH> <MASH_STEPS>
            if (source.Mash != null)
            {
                foreach (var mashStep in source.Mash.MashSteps)
                {
                    var mashStepDto = GetMashStepDto(mashStep);
                    recipeDto.MashSteps.Add(mashStepDto);
                }
            }
            //Fermentable
            if (source.Fermentables != null)
            {
                //var mashStep = recipeDto.MashSteps.FirstOrDefault();
                var mashStep = GetMashStepDto(recipeDto,(int)recipeDto.TotalBoilTime);
                foreach (var fermentable in source.Fermentables)
                {
                    
                    if (mashStep != null)
                    {
                        var fermentableStepDto = GetFermentableStepDto(fermentable, mashStep);
                        mashStep.Fermentables.Add(fermentableStepDto);
                    }
                }
            }

            //Hops
            if (source.Hops != null)
            {
                foreach (var hop in source.Hops)
                {
                    var time = (int)double.Parse(hop.Time, CultureInfo.InvariantCulture);
                    var hopStepDto = GetHopStepDto(hop);
                    if (string.Equals(hop.Use, "Boil", StringComparison.OrdinalIgnoreCase) || hop.Use == "")
                    {
                        var boilStep = GetBoilStepDto(recipeDto, time);
                        if (hopStepDto != null)
                            boilStep.Hops.Add(hopStepDto);
                    }

                    if (string.Equals(hop.Use, "First Wort"))
                    {
                        //TODO: add support for first wort.
                    }

                    if (string.Equals(hop.Use, "Mash") && string.Equals(hop.Use, "Aroma"))
                    {
                        var mashStep = GetMashStepDto(recipeDto, time);
                        if (hopStepDto != null)
                            mashStep.Hops.Add(hopStepDto);
                    }

                    if (hop.Use == "Dry Hop")
                    {
                        var fermentationStep = GetFermentationStepDto(recipeDto, time);
                        if (hopStepDto != null)
                            fermentationStep.Hops.Add(hopStepDto);
                    }
                }
            }

            if (source.Miscs != null)
            {
                foreach (var misc in source.Miscs)
                {
                    int time = 0;
                    if(misc.Time.Any())
                        time = (int)double.Parse(misc.Time, CultureInfo.InvariantCulture);
                    var othersStepDto = GetOthersStepDto(misc);
                    if (string.Equals(misc.Use, "Boil", StringComparison.OrdinalIgnoreCase))
                    {
                        var boilStep = GetBoilStepDto(recipeDto, time);
                        if (othersStepDto != null)
                            boilStep.Others.Add(othersStepDto);
                    }
                    if (string.Equals(misc.Use, "Mash", StringComparison.OrdinalIgnoreCase))
                    {
                        var mashStep = GetMashStepDto(recipeDto, time);
                        if (othersStepDto != null)
                            mashStep.Others.Add(othersStepDto);
                    }
                    if (string.Equals(misc.Use, "Primary", StringComparison.OrdinalIgnoreCase) || string.Equals(misc.Use, "Secondary", StringComparison.OrdinalIgnoreCase))
                    {
                        var fermentationStepDto = GetFermentationStepDto(recipeDto, time);
                        if (othersStepDto != null)
                            fermentationStepDto.Others.Add(othersStepDto);
                    }
                    if (string.Equals(misc.Use, "Bottling", StringComparison.OrdinalIgnoreCase))
                    {
                        //TODO: botteling step, beta shit.
                    }
                }
            }

            if (source.Yeasts != null)
            {
                foreach (var yeast in source.Yeasts)
                {
                    var yeastStepDto = GetYeastStepDto(yeast);
                    var inSecondary = string.Equals(yeast.Add_To_Secondary, "false",StringComparison.OrdinalIgnoreCase);
                    if (!inSecondary)
                    {
                        var fermentationStepDto = recipeDto.FermentationSteps.FirstOrDefault() ?? GetFermentationStepDto(recipeDto, 0);
                        fermentationStepDto.Yeasts.Add(yeastStepDto);
                    }
                    else
                    {
                        var fermentationStepDto = recipeDto.FermentationSteps.Skip(1).FirstOrDefault() ?? GetFermentationStepDto(recipeDto, 0);
                        fermentationStepDto.Yeasts.Add(yeastStepDto);
                    }
                    
                }
            }

            SetStepNumber(recipeDto);

            return recipeDto;
        }

        private void SetStepNumber(RecipeDto recipeDto)
        {
            var stepNumber = 1;
            foreach (var mashStep in recipeDto.MashSteps)
            {
                mashStep.StepNumber = stepNumber;
                stepNumber++;
            }
            recipeDto.SpargeStep.StepNumber = stepNumber;
            stepNumber++;
            foreach (var boilStep in recipeDto.BoilSteps.OrderByDescending(b => b.Length))
            {
                boilStep.StepNumber = stepNumber;
                stepNumber++;
            }
            
            foreach (var fermentationStep in recipeDto.FermentationSteps)
            {
                fermentationStep.StepNumber = stepNumber;
                stepNumber++;
            }
        }


        private static FermentationStepDto GetFermentationStepDto(RecipeDto recipeDto, int time)
        {
            var fermentationStep = recipeDto.FermentationSteps.SingleOrDefault(f => f.Length == time/24);
            if (fermentationStep != null) return fermentationStep;
            fermentationStep = new FermentationStepDto();
            fermentationStep.Length = time;
            fermentationStep.Hops = new List<HopStepDto>();
            fermentationStep.Fermentables = new List<FermentableStepDto>();
            fermentationStep.Others = new List<OtherStepDto>();
            fermentationStep.Yeasts = new List<YeastStepDto>();
            return fermentationStep;
        }

        private static MashStepDto GetMashStepDto(MashStep mashStep)
        {
            var mashStepDto = new MashStepDto();
            mashStepDto.Length = (mashStep.StepTime.Any()) ? decimal.Parse(mashStep.StepTime,CultureInfo.InvariantCulture) : 0;
            //TODO: make double???
            mashStepDto.Temperature = (int)double.Parse(mashStep.StepTemp, CultureInfo.InvariantCulture);
            mashStepDto.Type = mashStep.Type;
            mashStepDto.Notes = mashStep.Name;
            mashStepDto.Hops = new List<HopStepDto>();
            mashStepDto.Fermentables = new List<FermentableStepDto>();
            mashStepDto.Others = new List<OtherStepDto>();
            return mashStepDto;
        }

        private static MashStepDto GetMashStepDto(RecipeDto recipeDto, int time)
        {
            var mashStepDto = recipeDto.MashSteps.FirstOrDefault(m => m.Length == time);
            if (mashStepDto != null) return mashStepDto;
            mashStepDto = new MashStepDto();
            mashStepDto.Length = time;
            mashStepDto.Hops = new List<HopStepDto>();
            mashStepDto.Fermentables = new List<FermentableStepDto>();
            mashStepDto.Others = new List<OtherStepDto>();
            recipeDto.MashSteps.Add(mashStepDto);
            return mashStepDto;
        }

        private static BoilStepDto GetBoilStepDto(RecipeDto recipeDto, int time)
        {
            var boilStep = recipeDto.BoilSteps.SingleOrDefault(b => b.Length == time);
            if (boilStep != null) return boilStep;
            boilStep = new BoilStepDto();
            boilStep.Length = time;
            boilStep.Hops = new List<HopStepDto>();
            boilStep.Fermentables = new List<FermentableStepDto>();
            boilStep.Others = new List<OtherStepDto>();
            recipeDto.BoilSteps.Add(boilStep);
            return boilStep;
        }

        private FermentableStepDto GetFermentableStepDto(Fermentable fermentable, MashStepDto mashStep)
        {
            var fermentableDto = _fermentableElasticsearch.Search(fermentable.Name, 0, 1).FirstOrDefault();
            if (fermentableDto == null) return null;
            var fermentableStepDto = Mapper.Map<FermentableDto, FermentableStepDto>(fermentableDto);
            fermentableStepDto.StepNumber = mashStep.StepNumber;
            double amount = double.Parse(fermentable.Amount, CultureInfo.InvariantCulture);
            fermentableStepDto.Amount = (int)Math.Round(amount * 1000, 0);
            return fermentableStepDto;
        }

        private HopStepDto GetHopStepDto(Hop hop)
        {
            var hopForms = _hopRepository.GetHopForms();
            foreach (var hopForm in hopForms)
            {
                Log.Debug("HopFrom: " + hopForm.Name);
            }
            Log.Debug("hop.Form: " + hop.Form);
            var hopDto = _hopElasticsearch.Search(hop.Name, 0, 1).FirstOrDefault();
            if (hopDto == null) return null;
            var hopStepDto = Mapper.Map<HopDto, HopStepDto>(hopDto);
            hopStepDto.HopId = hopDto.Id;
            double alpha = double.Parse(hop.Alpha, CultureInfo.InvariantCulture);
            hopStepDto.AAValue = alpha;
            double amount = double.Parse(hop.Amount, CultureInfo.InvariantCulture);
            hopStepDto.Amount = (int)Math.Round(amount * 1000, 0);
            hopStepDto.HopForm =
                Mapper.Map<HopForm, DTO>(hopForms.FirstOrDefault(
                    h => string.Equals(h.Name, hop.Form, StringComparison.OrdinalIgnoreCase)));
            return hopStepDto;
        }

        private OtherStepDto GetOthersStepDto(Misc misc)
        {
            var otherDto = _otherElasticsearch.Search(misc.Name, 0, 1).FirstOrDefault();
            if (otherDto == null) return null;
            var otherStepDto = Mapper.Map<OtherDto, OtherStepDto>(otherDto);
            double amount = double.Parse(misc.Amount, CultureInfo.InvariantCulture);
            otherStepDto.Amount = (int)Math.Round(amount * 1000, 0);
            return otherStepDto;
        }

        private YeastStepDto GetYeastStepDto(Yeast yeast)
        {
            var yeastDto = _yeastElasticsearch.Search(yeast.Name,0,1).FirstOrDefault();
            if (yeastDto == null) return null;
            var yeastStepDto = Mapper.Map<YeastDto, YeastStepDto>(yeastDto);

            double amount = (yeast.Amount != null) ? double.Parse(yeast.Amount, CultureInfo.InvariantCulture) : 0;
            yeastStepDto.Amount = (int) Math.Round(amount, 0);
            return yeastStepDto;
        }
    }
}
