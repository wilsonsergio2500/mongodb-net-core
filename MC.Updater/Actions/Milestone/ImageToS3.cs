﻿using MC.Interfaces.Repository;
using Mdls = MC.Models;
using System.Threading.Tasks;
using MC.Updater.Utils;
using MC.AmazonStoreS3.Providers;

namespace MC.Updater.Actions.Milestone
{
    public static class ImageToS3
    {

        public static async Task<bool> Do() {

            IMilestoneRepository milestoneRepository = DI.ServiceProvider.GetServiceInstance<IMilestoneRepository>();
            IAmazonS3ImageProvider amazonS3ImageProvider = DI.ServiceProvider.GetServiceInstance<IAmazonS3ImageProvider>(); 

            foreach (Mdls.Milestone milestone in await milestoneRepository.GetAll()) {

                if (milestone.Image != null && milestone.Image.StartsWith("data:image/png;base64,")) {

                    string path = await amazonS3ImageProvider.Add($"{milestone.UserId}/{milestone.id}/img.png", milestone.Image);

                   bool saved =  await milestoneRepository.SetImage(milestone.id, path);
                }

                

            }
            return true;
        }
    }
}
