using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Email.Utils
{

  
    public static class EmailTemplate
    {

        const string basePath = "http://landmarkapp-dev.us-west-2.elasticbeanstalk.com/";
        public static string INVITE_TEMPLATE 
         {
                get {

                string btnTemplate = @"<div style='text-align:center'>

                    <div style='background-color:rgb(33,150,243); color: rgb(255,255,255); display: inline-block; padding: 0 6px; border-radius: 2px; margin: 6px 8px; font-weight: 500; font-size:14px; min-height: 36px; min-width: 88px; vertical-align: middle; align-items: center; line-height:36px; letter-spacing: 0.010em; box-shadow: 0px 7px 8px -4px rgba(0,0,0,0.2), 0px 12px 17px 2px rgba(0,0,0,0.14), 0px 5px 22px 4px rgba(0,0,0,0.12); text-decoration:none; cursor: pointer; text-transform: uppercase' >
                     <a style='text-decoration:none; color: white' href = 'https://www.google.com/' target = '_blank' > Complete Invite </ a >

                </div >";

                    return btnTemplate;
                }
         }

        public static string getInviteTemplate(string invitationId) {
            return @"<div style='text-align:center'>

                    <div style='text-transform:uppercase; width: 100%; background-color:#4CAF50; color:white; padding:10px; border-radius:3px; font-weight:bold; font-size:20px'>
                    Landmark
                    </div>

                            <div style='
                                width: 400px;
                                margin: auto;
                                padding: 10px;
                                text-align: justify;
                                font-family: ""Open Sans"", Verdana, Arial, Helvetica, sans-serif;
                                margin-top: 20px;
                                margin-bottom: 20px;
                                letter-spacing: 0.02em;
                                opacity: 0.7;
                                line-height: 25px;'>
                                        <p>
                                        A friend has invited you to participate in the Landmark application, share your Milestone with other friends and feel proud about your accomplishments.
                                        </p>
                                        <p>
                                        This application is also a great tool for tracking certification completes and classes finalized
                                        </p>
                                        <p>
                                        Share Proffesional and Personal goals, and give kudos to your friends accomplishments.
                                        </p>
                                        <p>
                                        Complete your invite by clicking the link below
                                        </p>
                            </div>

                    <div style='background-color:rgb(33,150,243); color: rgb(255,255,255); display: inline-block; padding: 0 6px; border-radius: 2px; margin: 6px 8px; font-weight: 500; font-size:14px; min-height: 36px; min-width: 88px; vertical-align: middle; align-items: center; line-height:36px; letter-spacing: 0.010em; box-shadow: 0px 7px 8px -4px rgba(0,0,0,0.2), 0px 12px 17px 2px rgba(0,0,0,0.14), 0px 5px 22px 4px rgba(0,0,0,0.12); text-decoration:none; cursor: pointer; text-transform: uppercase' >"
                    + $"<a style='text-decoration:none; color: white' href = '{basePath}#/landmark/complete/{invitationId}' target = '_blank' > Complete Invite </a></div >";

        }     
    }
 }

