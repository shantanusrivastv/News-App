kubectl delete deployment pressford-news-client
kubectl delete svc pressford-news-client
timeout /t 2
docker rmi pressford-news-client