#!/bin/sh

ethdir=/home/eth
bootnode=`cat $ethdir/bootnode`

geth -datadir $ethdir/data init $ethdir/genesis.json > $ethdir/eth.log 2>&1

geth --datadir /home/eth/data --networkid 13337  --http --http.api personal,eth,net,web3 --http.corsdomain https://remix.ethereum.org console --http.addr `hostname -i` --unlock 0 --password password --allow-insecure-unlock --bootnodes $bootnode