#!/bin/bash

openssl genrsa -des3 -passout pass:x -out server.pass.key 2048
openssl rsa -passin pass:x -in server.pass.key -out /etc/cert/cert.key
rm server.pass.key
openssl req -new -key /etc/cert/cert.key -out server.csr -subj "/C=UK/ST=Warwickshire/L=Leamington/O=OrgName/OU=IT Department/CN=example.com"
openssl x509 -req -days 365 -in server.csr -signkey /etc/cert/cert.key -out /etc/cert/cert.crt
sleep 5