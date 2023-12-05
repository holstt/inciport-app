# Nginx container used in production (only)
# Needs custom config in order to serve both website and webservice
# from the same domain but two different containers.

FROM nginx

COPY ./config /etc/nginx/conf.d

ENTRYPOINT ["nginx", "-g", "daemon off;"]
