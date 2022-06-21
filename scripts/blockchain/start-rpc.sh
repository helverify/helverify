#!/bin/sh

ethdir=/home/eth

geth --datadir $ethdir/data --networkid 13337 --port $PORT --syncmode full --nat extip:`dig +short host.docker.internal` --http --http.addr `hostname -i` --http.api personal,eth,net,web3 --http.corsdomain https://remix.ethereum.org --unlock `cat $ethdir/address` --password $ethdir/password --allow-insecure-unlock --bootnodes= > $ethdir/eth.log 2>&1 &

until [ -e $ethdir/data/geth.ipc ]
do
    sleep 5
done

for element in $(jq '.nodes[]' $ethdir/nodes.json);
do
    geth attach $ethdir/data/geth.ipc --exec "admin.addPeer($element)"
done
