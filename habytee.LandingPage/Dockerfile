FROM nginx:alpine

ENV REPLACEMENT_NAME="APP_DOMAIN"

COPY nginx.conf /etc/nginx/conf.d/default.conf

WORKDIR /usr/share/nginx/html
COPY index.html index.html
COPY css/ css/
COPY font/ font/
COPY img/ img/
RUN chown -R nginx:nginx *

EXPOSE 80

CMD envsubst '$APP_DOMAIN' < /usr/share/nginx/html/index.html > /tmp/index.html && mv /tmp/index.html /usr/share/nginx/html/index.html && nginx -g 'daemon off;'
