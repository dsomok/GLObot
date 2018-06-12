FROM nginx

RUN apt-get update && \
    apt-get install -y openssl

COPY nginx.conf /etc/nginx/conf.d/reverseproxy.conf
ADD entrypoint.sh /etc/nginx/entrypoint.sh

WORKDIR /etc/nginx

ENTRYPOINT ["/etc/nginx/entrypoint.sh"]