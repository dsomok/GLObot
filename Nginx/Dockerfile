FROM nginx

RUN apt-get update && \
    apt-get install -y openssl

COPY nginx.conf /etc/nginx/conf.d/reverseproxy.conf
ADD generate-certificate.sh /etc/nginx/generate-certificate.sh


CMD [ "/etc/nginx/generate-certificate.sh", "&&", "/etc/nginx/nginx" ]