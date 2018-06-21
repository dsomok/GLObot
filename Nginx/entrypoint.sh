#!/bin/bash

openssl req -newkey rsa:2048 -sha256 -nodes -keyout YOURPRIVATE.key -x509 -days 365 -out YOURPUBLIC.pem -subj "/CN=$VIRTUAL_HOST"

cp YOURPUBLIC.pem /etc/cert/cert.pem
cp YOURPRIVATE.key /etc/cert/cert.key
nginx -g "daemon off;" 