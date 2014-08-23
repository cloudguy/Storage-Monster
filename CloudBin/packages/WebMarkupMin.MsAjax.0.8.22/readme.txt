

   ----------------------------------------------------------------------
             README file for Web Markup Minifier: MS Ajax 0.8.22

   ----------------------------------------------------------------------

          Copyright 2014 Andrey Taritsyn - http://www.taritsyn.ru
		  
		  
   ===========
   DESCRIPTION
   ===========   
   WebMarkupMin.MsAjax contains 2 minifier-adapters: 
   MsAjaxCssMinifier (for minification of CSS code) and 
   MsAjaxJsMinifier (for minification of JS code). These adapters 
   perform minification using the Microsoft Ajax Minifier 
   (http://ajaxmin.codeplex.com).
   
   =============
   RELEASE NOTES
   =============
   Added support of the Microsoft Ajax Minifier version 5.10.
   
   ====================
   POST-INSTALL ACTIONS
   ====================
   To make MsAjaxCssMinifier is the default CSS minifier and 
   MsAjaxJsMinifier is the default JS minifier, you need to make
   changes to the Web.config file. 
   In defaultMinifier attribute of element 
   \configuration\webMarkupMin\core\css must be set value equal to 
   MsAjaxCssMinifier, and in same attribute of element 
   \configuration\webMarkupMin\core\js - MsAjaxJsMinifier.

   =============
   DOCUMENTATION
   =============
   See more detailed information on CodePlex -
   http://webmarkupmin.codeplex.com