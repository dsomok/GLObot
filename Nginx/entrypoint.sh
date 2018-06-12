#!/bin/bash

openssl genpkey -algorithm RSA -pkeyopt rsa_keygen_bits:2048 -out server.key
openssl req -new -key server.key -out server.csr -subj "/CN=$VIRTUAL_HOST"
openssl x509 -req -days 365 -in server.csr -signkey server.key -out server.crt
cp server.crt /etc/cert/cert.crt
cp server.key /etc/cert/cert.key
nginx -g "daemon off;"