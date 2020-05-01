using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEA_Application.CustomModel
{
    public static  class EmailDesign
    {

        
        public static string SignupEmailTemplate(string Code, string FullName)
        {
            string template = "";

            template += "<html xmlns = 'http://www.w3.org/1999/xhtml' xmlns: v = 'urn:schemas-microsoft-com:vml' xmlns: o = 'urn:schemas-microsoft-com:office:office' >";
            template += "<head>";
            template += "<meta charset = 'UTF-8' >";
            template += "<meta http - equiv = 'X -UA-Compatible' content = 'IE=edge' >";
            template += "<meta name = 'viewport' content = 'width=device-width, initial-scale=1' >";
            template += "<title > Reset Your Lingo Password </title >";
            template += "<link href = 'https://fonts.googleapis.com/css?family=Asap:400,400italic,700,700italic' rel = 'stylesheet' type = 'text/css' >";
            template += "</head>";
            template += "<body style = '-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;background-color: #48525e; height: 100%; margin: 0; padding: 0; width: 100%' > <center >";
            template += "<table align = 'center' border = '0' cellpadding = '0' cellspacing = '0' height = '100%' id = 'bodyTable' style = 'border-collapse: collapse; mso-table-lspace: 0;mso-table-rspace: 0; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; background-color: #48525e; height: 100%; margin: 0; padding: 0; width:100%' width = '100%' >";
            template += "<tr>";
            template += "<td align = 'center' id = 'bodyCell' style = 'mso-line-    height-rule: exactly;-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; border-top: 0;height: 100%; margin: 0; padding: 0; width: 100%' valign = 'top' >";
            template += "<table border = '0' cellpadding = '0' cellspacing = '0' class='templateContainer' style='border-collapse: collapse; mso-table-lspace: 0; mso-table-rspace: 0;-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; max-width:600px; border: 0' width='100%'>";
            template += "<tr>";
            template += "<td id = 'templatePreheader' style='mso-line-height-rule: exactly;-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; background-color: #48525e;border-top: 0; border-bottom: 0; padding-top: 16px; padding-bottom: 8px' valign='top'>";
            template += "<table border = '0' cellpadding='0' cellspacing='0' class='mcnTextBlock' style='border-collapse: collapse; mso-table-lspace: 0;mso-table-rspace: 0; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;min-width:100%;' width='100%'>";
            template += "<tbody class='mcnTextBlockOuter'>";
            template += "<tr>";
            template += "<td class='mcnTextBlockInner' style='mso-line-height-rule: exactly;-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%' valign='top'>";
            template += "<table align = 'left' border='0' cellpadding='0' cellspacing='0' class='mcnTextContentContainer' style='border-collapse: collapse; mso-table-lspace: 0;mso-table-rspace: 0; -ms-text-size-adjust: 100%; -webkit-text-size-adjust:100%; min-width:100%;' width='100%'>";
            template += "<tbody>";
            template += "<tr>";
            template += "<td class='mcnTextContent' style='mso-line-height-rule: exactly; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; word-break: break-word; color: #2a2a2a; font-family: Asap, Helvetica, sans-serif; font-size: 12px; line-height: 150%; text-align: left; padding-top:9px; padding-right: 18px; padding-bottom: 9px; padding-left: 18px;' valign='top'>";
            //     template += "<a href = 'https://mp.org/' style='mso-line-height-rule: exactly; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; color: #2a2a2a; font-weight: normal; text-decoration: none' target='_blank' >";
            //   template += "<img align = 'none' alt='Multi Platform Recruiting' height='32' src='https://mpr24.com/assets/layouts/layout/img/logo.png' style='-ms-interpolation-mode: bicubic; border: 0; outline: none;text-decoration: none; height: auto; width: 107px; height: 32px; margin: 0px;' width='107' />";
           template += "</td></tr></tbody></table></td></tr></tbody></table></td></tr><tr>";
            template += "<td id = 'templateHeader' style='mso-line-height-rule: exactly;-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; background-color: #f7f7ff; border-top: 0; border-bottom: 0; padding-top: 16px; padding-bottom: 0' valign='top'>";
            template += "<table border = '0' cellpadding='0' cellspacing='0' class='mcnImageBlock' style='border-collapse: collapse; mso-table-lspace: 0; mso-table-rspace: 0;";
            template += "-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; min-width:100%;' width='100%'>";
            template += "<tbody class='mcnImageBlockOuter'><tr><td class='mcnImageBlockInner' style='mso-line-height-rule: exactly;-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; padding:0px' valign='top'>";
            template += "<table align = 'left' border='0' cellpadding='0' cellspacing='0' class='mcnImageContentContainer' style='border-collapse: collapse; mso-table-lspace: 0;";
            template += "mso -table-rspace: 0; -ms-text-size-adjust: 100%; -webkit-text-size-adjust:100%; min-width:100%;' width='100%'>";
            template += "<tbody><tr><td class='mcnImageContent' style='mso-line-height-rule: exactly;";
            template += " -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; padding-right: 0px;";
            template += " padding-left: 0px; padding-top: 0; padding-bottom: 0; text-align:center;' valign='top'>";
            template += " <a class='' href='http://www.sea-ngsipccom/' style='mso-line-height-rule:";
            template += " exactly; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; color:";
            template += "#0f9200; font-weight: normal; text-decoration: none' target='_blank' title=''>";
            template += "<a class='' href='http://www.sea-ngsipccom//' style='mso-line-height-rule:";
            template += "exactly; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; color:";
            template += "#0f9200; font-weight: normal; text-decoration: none' target='_blank' title=''>";
            template += "<img align = 'center' class='mcnImage' src='https://static.lingoapp.com/assets/images/email/il-password-reset@2x.png' style='-ms-interpolation-mode: bicubic; border: 0; height: auto; outline: none;";
            template += "text -decoration: none; vertical-align: bottom; max-width:1200px; padding-bottom:";
            template += "0; display: inline !important; vertical-align: bottom;' width='600'></img>";
            template += "</a></a></td></tr></tbody></table></td></tr></tbody></table></td></tr><tr>";
            template += " <td id='templateBody' style='mso-line-height-rule: exactly;";
            template += "-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; background-color: #f7f7ff;";
            template += "border-top: 0; border-bottom: 0; padding-top: 0; padding-bottom: 0' valign='top'>";
            template += "<table border = '0' cellpadding='0' cellspacing='0' class='mcnTextBlock' style='border-collapse: collapse; mso-table-lspace: 0; mso-table-rspace: 0;";
            template += "-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; min-width:100%;' width='100%'>";
            template += "<tbody class='mcnTextBlockOuter'>";
            template += "<tr>";
            template += " <td class='mcnTextBlockInner' style='mso-line-height-rule: exactly;";
            template += "-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%' valign='top'>";
            template += "<table align = 'left' border='0' cellpadding='0' cellspacing='0' class='mcnTextContentContainer' style='border-collapse: collapse; mso-table-lspace: 0;";
            template += " mso-table-rspace: 0; -ms-text-size-adjust: 100%; -webkit-text-size-adjust:";
            template += "100%; min-width:100%;' width='100%'>";
            template += " <tbody>";
            template += " <tr>";
            template += "<td class='mcnTextContent' style='mso-line-height-rule: exactly;";
            template += "-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; word-break: break-word;";
            template += "color:#2a2a2a; font-family: Asap, Helvetica, sans-serif; font-size: 16px;";
            template += "line-height: 150%; text-align: center; padding-top:9px; padding-right: 18px;";
            template += "padding-bottom: 9px; padding-left: 18px;' valign='top'>";

            template += " <h1 class='null' style='color:#2a2a2a;font-family:Asap, Helvetica,";
            template += "sans-serif; font-size: 32px; font-style: normal; font-weight: bold; line-height:";
            template += "125%; letter-spacing: 2px; text-align: center; display: block; margin: 0;";
            template += "padding:0'><span style='text-transform:uppercase'>Hi,</span></h1>";


            template += "<h2 class='null' style='color: #2a2a2a; font-family: Asap, Helvetica,";
            template += "sans-serif; font-size: 24px; font-style: normal; font-weight: bold; line-height:";
            template += "125%; letter-spacing: 1px; text-align: center; display: block; margin: 0;";
            template += "padding:0'><span style='text-transform:uppercase'>" + FullName + "</span></h2>";

            template += " </td></tr></tbody></table></td></tr></tbody></table>";
            template += "<table border = '0' cellpadding='0' cellspacing='0' class='mcnTextBlock' style='border-collapse: collapse; mso-table-lspace: 0; mso-table-rspace:";
            template += " 0; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;";
            template += "min-width:100%;' width='100%'>";
            template += " <tbody class='mcnTextBlockOuter'>";
            template += " <tr>";
            template += " <td class='mcnTextBlockInner' style='mso-line-height-rule: exactly;";
            template += " -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%' valign='top'>";
            template += " <table align = 'left' border='0' cellpadding='0' cellspacing='0' class='mcnTextContentContainer' style='border-collapse: collapse; mso-table-lspace: 0;";
            template += "mso-table-rspace: 0; -ms-text-size-adjust: 100%; -webkit-text-size-adjust:";
            template += "100%; min-width:100%;' width='100%'>";
            template += "<tbody>";
            template += " <tr>";
            template += " <td class='mcnTextContent' style='mso-line-height-rule: exactly;";
            template += "-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; word-break: break-word;";
            template += "color: #2a2a2a; font-family: Asap, Helvetica, sans-serif; font-size: 16px;";
            template += "line-height: 150%; text-align: center; padding-top:9px; padding-right: 18px;";
            template += " padding-bottom: 9px; padding-left: 18px;' valign='top'><b>Welcome to SEA | Smarter Education Analytics</b>";
            template += "<br></br>";
            template += " </td>";
            template += " </tr>";
            template += "</tbody>";
            template += " </table>";
            template += " </td>";
            template += "</tr>";
            template += " </tbody>";
            template += "</table>";
            template += " <table border = '0' cellpadding='0' cellspacing='0' class='mcnButtonBlock' style='border-collapse: collapse; mso-table-lspace: 0;";
            template += "mso-table-rspace: 0; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;";
            template += " min-width:100%;' width='100%'>";
            template += " <tbody class='mcnButtonBlockOuter'>";
            template += " <tr>";
            template += "<td align = 'center' class='mcnButtonBlockInner' style='mso-line-height-rule:";
            template += " exactly; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;";
            template += "padding-top:18px; padding-right:18px; padding-bottom:18px; padding-left:18px;' valign='top'>";
            template += "<table border = '0' cellpadding='0' cellspacing='0' class='mcnButtonBlock' style='border-collapse: collapse; mso-table-lspace: 0; mso-table-rspace: 0;";
            template += "-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; min-width:100%;' width='100%'>";
            template += "<tbody class='mcnButtonBlockOuter'>";
            template += "<tr>";
            template += " <td align = 'center' class='mcnButtonBlockInner' style='mso-line-height-rule:";
            template += "exactly; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;";
            template += "padding-top:0; padding-right:18px; padding-bottom:18px; padding-left:18px;' valign='top'>";
            template += "<table border = '0' cellpadding='0' cellspacing='0' class='mcnButtonContentContainer' style='border-collapse: collapse; mso-table-lspace: 0;";
            template += "mso-table-rspace: 0; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; ";
            template += "border-collapse: separate !important;border-radius: 48px;background-color:";
            template += "#0f9200;'>";
            template += "<tbody>";
            template += "  <tr>";
            template += " <td align = 'center' class='mcnButtonContent' style='mso-line-height-rule:";
            template += " exactly; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;";
            template += "font-family: Asap, Helvetica, sans-serif; font-size: 16px; padding-top:24px;";
            template += " padding-right:48px; padding-bottom:24px; padding-left:48px;' valign='middle'>";
            template += "<a class='mcnButton ' href='" + "' style='mso-line-height-rule: exactly;";
            template += "-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; display: block; color: #0f9200;";
            template += "font-weight: normal; text-decoration: none; font-weight: normal;letter-spacing:";
            template += "1px;line-height: 100%;text-align: center;text-decoration: none;color:";
            template += "#FFFFFF; text-transform:uppercase;' target='_blank' title='Click Here To";
            template += "Activate Account'>" + Code + " </a>";
            template += "</td></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table>";
            
            template += "<table border = '0' cellpadding='0' cellspacing='0' class='mcnImageBlock' style='border-collapse: collapse; mso-table-lspace: 0; mso-table-rspace: 0;";
            template += "-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; min-width:100%;' width='100%'>";
            template += "<tbody class='mcnImageBlockOuter'>";
            template += "<tr>";
            template += "<td class='mcnImageBlockInner' style='mso-line-height-rule: exactly;";
            template += " -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; padding:0px' valign='top'>";
            template += "<table align = 'left' border='0' cellpadding='0' cellspacing='0' class='mcnImageContentContainer' style='border-collapse: collapse; mso-table-lspace: 0;";
            template += "mso-table-rspace: 0; -ms-text-size-adjust: 100%; -webkit-text-size-adjust:";
            template += " 100%; min-width:100%;' width='100%'>";
            template += " <tbody>";
            template += " <tr>";
            template += "<td class='mcnImageContent' style='mso-line-height-rule: exactly;";
            template += "-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; padding-right: 0px;";
            template += " padding-left: 0px; padding-top: 0; padding-bottom: 0; text-align:center;' valign='top'></td>";
            template += " </tr></tbody></table></td></tr></tbody></table></td></tr><tr>";
            template += "<td id='templateFooter' style='mso-line-height-rule: exactly;";
            template += "-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; background-color: #48525e;";
            template += " border-top: 0; border-bottom: 0; padding-top: 8px; padding-bottom: 80px' valign='top'>";
            template += " <table border = '0' cellpadding='0' cellspacing='0' class='mcnTextBlock' style='border-collapse: collapse; mso-table-lspace: 0; mso-table-rspace: 0;";
            template += "-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; min-width:100%;' width='100%'>";
            template += " <tbody class='mcnTextBlockOuter'>";
            template += "<tr>";
            template += "<td class='mcnTextBlockInner' style='mso-line-height-rule: exactly;";
            template += " -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%' valign='top'>";
            template += " <table align = 'center' bgcolor='#F7F7FF' border='0' cellpadding='32' cellspacing='0' class='card' style='border-collapse: collapse; mso-table-lspace: 0;";
            template += "mso-table-rspace: 0; -ms-text-size-adjust: 100%; -webkit-text-size-adjust:";
            template += "100%; background:#F7F7FF; margin:auto; text-align:left; max-width:600px;";
            template += " font-family: Asap, Helvetica, sans-serif;' text-align='left' width='100%'>";
            template += " <tr>";
            template += "<td style='mso-line-height-rule: exactly; -ms-text-size-adjust: 100%;";
            template += "-webkit-text-size-adjust:100%'>";


            //footer
            template += " <h3 style='color: #2a2a2a; font-family: Asap, Helvetica, sans-serif;";
            template += "font-size: 20px; font-style: normal; font-weight: normal; line-height: 125%;";
            template += "letter-spacing: normal; text-align: center; display: block; margin: 0; padding:";
            template += "0; text-align: left; width: 100%; font-size: 16px; font-weight: bold; '> ";
            template += "What is NGS ?</h3>";

            template += "<p style ='margin:10px 0; padding: 0; mso-line-height-rule: exactly;";
            template += "-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; color: #2a2a2a;";
            template += "font-family: Asap, Helvetica, sans-serif; font-size: 12px; line-height: 150%;";
            template += "text-align: left; text-align: left; font-size: 14px; '>The educational system at National Grammar School stimulates intellectual curiosity and exploration spirit. The school ethos encourages open mindedness and the release of intellectual potential. Students are encouraged to be self-reliant critical thinkers, who realize that learning is a lifelong process.";
            template += "  </p>";

            template += "</body>";

            template += "</html>";

            return template;
        }

    }
}