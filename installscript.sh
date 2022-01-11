#!/usr/bin/bash

echo "V--------------------Updating VM--------------------V"
#sudo apt-get update

#Get code from repository
sudo rm -rf ~/WinTrackWebApp
git clone https://github.com/Unknown-GH/WinTrackWebApp.git ~/WinTrackWebApp

#Change appsettings.json connection string
sudo sed -i 's/Server=(localdb)\\\\mssqllocaldb;Database=WinTrackDatabase;Trusted_Connection=True;MultipleActiveResultSets=true/Server=localhost;Database=WinTrackDatabase;Trusted_Connection=True;MultipleActiveResultSets=true;User id=sa;Password=A3secwintrack6;Integrated Security=false/g' ~/WinTrackWebApp/WinTrackWebApp/appsettings.json

#Install dependencies
echo "V--------------------Installing dependencies--------------------V"
sudo snap install docker
sudo snap install dotnet-sdk --classic --channel=3.1
sudo apt install nginx -y

#Setup database
echo "V--------------------Setting up database--------------------V"
if [[ $(sudo docker ps -a | grep 'Exited.*database') ]]; then
        sudo docker start database
elif ! [[ $(sudo docker ps -a | grep 'database') ]]; then
 	sudo docker pull mcr.microsoft.com/mssql/server:2019-latest
 	sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=A3secwintrack6" \
      		-p 1433:1433 --name database -h database \
      		-d mcr.microsoft.com/mssql/server:2019-latest
else
	echo "Database had already been succesfully set up"
fi

#Setup nginx
sudo -- sh -c 'echo "server {
	listen 443 ssl http2;
	listen [::]:443;

	root /home/student/WinTrackWebApp;
	index index.html index.htm index.nginx-debian.html;
	server_name _;
	server_tokens off;

	ssl_certificate		/home/student/server-cert.crt;
	ssl_certificate_key	/home/student/server-cert.key;
	ssl_session_cache	builtin:1000 shared:SSL:10m;
	ssl_protocols		TLSv1.3;
	ssl_prefer_server_ciphers on;

	location / {
		proxy_set_header	Host \$host;
		proxy_set_header	X-Real_IP \$remote_addr;
		proxy_set_header	X-Forwarded-For \$proxy_add_x_forwarded_for;
		proxy_set_header	X-Forwarded-Proto \$scheme;
		proxy_pass		http://localhost:5000;
	}
}

server {
	listen 80;
	server_name _;
	return 301 https://\$host\$request_uri;
}" > /etc/nginx/sites-available/default'
if [[ ! -f ~/server-cert.key ]]; then
	echo test
	openssl req -x509 -newkey rsa:4096 -keyout ~/server-cert.key -out ~/server-cert.crt -sha256 -days 365 -nodes
fi

sudo service nginx restart

#Build Web App
echo "V--------------------Building Web App--------------------V"
dotnet publish ~/WinTrackWebApp/WinTrackWebApp

#Run Web App
echo "V--------------------Running Web App--------------------V"
ip=$(hostname -I | sed 's/\s.*$//')
cd ~/WinTrackWebApp/WinTrackWebApp/bin/Debug/netcoreapp3.1/publish/
dotnet WinTrackWebApp.dll $ip
