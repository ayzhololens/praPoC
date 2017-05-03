#!/usr/bin/env node
var request = require('request');
var fs      = require('fs');
var config = require('./config.json');
var packageName = false;
var auth = "Basic " + new Buffer('ayzhololens' + ":" + 'holoVI$I0N').toString("base64");
const path = require('path');
 
if(!config.hololens)
{
	console.log("Invalid hololens in config file");
	process.exit();
}

//load config
var host = config.hololens;
console.log("connecting to " + host + "...");
process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";

//get list of packages from hololens
request({
		url :'https://' + host + '/api/app/packagemanager/packages',
		headers: { 
			"authorization" : auth
		}
	}	, function (error, response, packageBody) {
  //console.log(body);
  if(error)
  {
      console.log("Could not reach hololens.");
      process.exit();
  }

  if(packages = JSON.parse(packageBody))
  {
	  if(packages.InstalledPackages && packages.InstalledPackages.length)
	  {
	  	for(var i = 0; i < packages.InstalledPackages.length; i++)
	  	{
	  		//console.log(packages.InstalledPackages[i].PackageFullName);
	  		if(packages.InstalledPackages[i].Name.toString().match(/praeses/ig))
	  		{
	  			packageName = packages.InstalledPackages[i].PackageFullName;
	  			console.log('Found package: ', packageName);
	  		}
	  	}
	  	if(packageName)
	  	{
	  			//get list of files for package
	  			request({
	  					url: 'https://' + host + '/api/filesystem/apps/files?knownfolderid=LocalAppData&packagefullname=' + packageName + '&path=%5C%5CLocalState',
	  					headers: { 
								"authorization" : auth
							}
	  				}, function (error, response, fileResponseBody) {
            if(filesBody = JSON.parse(fileResponseBody))
            {	
              if(filesBody.Items && filesBody.Items.length)
              {
              	for(var f=0; f < filesBody.Items.length; f++)
              	{
              		var fileName = filesBody.Items[f].Name;
              		//skip directories
              		if(filesBody.Items[f].Type == 16)
              			continue;
              		//skip .dat files
              		else if(fileName.match(/\.dat/))
              			continue;
              			//console.log("Downloading " + fileName);

              			//download each file
              			request.get({
              				url: 'https://' + host + '/api/filesystem/apps/file\?knownfolderid\=LocalAppData\&filename\=' + fileName + '\&packagefullname\=' + packageName + '\&path\=%5C%5CLocalState', 
              				encoding: 'binary',
              				headers: { 
												"authorization" : auth
											}
              			}, function (err, response, body) {
									  		fs.writeFile(config.savepath + path.sep + this.fileName, body, 'binary', function(err) {
									  		//fs.writeFile("./files" + path.sep + this.fileName, body, 'binary', function(err) {
									  		console.log('saving ' + this.fileName);
									    	if(err)
									      	console.log(err);
									    	//else
									      	//console.log("The file was saved!");
									  }.bind({fileName: this.fileName})); 
									}.bind({fileName: fileName}));
              	}
              }
          	}else
          	{
          		console.log('could not parse file response');
          		process.exit();
          	}
          });

	  	}else
	  	{
	  		console.log("Could not find package, exiting");
	  		process.exit();
	  	}

	  }else
	  {
	  	console.log('Could not read packages');
	  }
	}else {
		console.log("Could not parse package response");
		process.exit();
	}

});
