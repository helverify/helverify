/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface Account {
  address?: string | null;
  funds?: string | null;
}

/**
 * Contains the parameters for ballot generation
 */
export interface BallotGenerationDto {
  /**
   * Number of ballots to be generated at once.
   * @format int32
   */
  numberOfBallots?: number;
}

export interface BigInteger {
  /** @format int32 */
  bitCount?: number;

  /** @format int32 */
  bitLength?: number;

  /** @format int32 */
  intValue?: number;

  /** @format int32 */
  intValueExact?: number;

  /** @format int64 */
  longValue?: number;

  /** @format int64 */
  longValueExact?: number;

  /** @format int32 */
  signValue?: number;
}

export interface Blockchain {
  id?: string | null;
  name?: string | null;
  registrations?: Registration[] | null;
}

export interface BlockchainDto {
  id?: string | null;
  name?: string | null;
  registrations?: RegistrationDto[] | null;
}

/**
 * Represents the ciphertext of an ElGamal cryptosystem
 */
export interface Cipher {
  /** First parameter of an ElGamal ciphertext */
  c?: string | null;

  /** Second parameter of an ElGamal ciphertext */
  d?: string | null;
}

export interface CipherTextDto {
  c?: string | null;
  d?: string | null;
}

export interface DHParameters {
  p?: BigInteger;
  g?: BigInteger;
  q?: BigInteger;
  j?: BigInteger;

  /** @format int32 */
  m?: number;

  /** @format int32 */
  l?: number;
  validationParameters?: DHValidationParameters;
}

export interface DHValidationParameters {
  /** @format int32 */
  counter?: number;
}

export interface Election {
  id?: string | null;
  name?: string | null;
  question?: string | null;
  options?: ElectionOption[] | null;
  p?: BigInteger;
  g?: BigInteger;
  publicKey?: BigInteger;
  blockchain?: Blockchain;
  dhParameters?: DHParameters;
  contractAddress?: string | null;
}

/**
 * Represents an Election
 */
export interface ElectionDto {
  /** Unique election identifier. */
  id?: string | null;

  /** Name of the election. */
  name?: string | null;

  /** Voting/election question */
  question?: string | null;

  /** Represents the voting options for this election. */
  options?: ElectionOptionDto[] | null;

  /** Prime p of the ElGamal cryptosystem. */
  p?: string | null;

  /** Generator g of the ElGamal cryptosystem. */
  g?: string | null;

  /** Blockchain Identifier */
  blockchainId?: string | null;

  /** Election public key */
  publicKey?: string | null;

  /** Address of the smart contract for the election on the Ethereum blockchain. */
  contractAddress?: string | null;
}

export interface ElectionOption {
  name?: string | null;
}

/**
 * Represents a single option in an election or vote.
 */
export interface ElectionOptionDto {
  /** Candidate/Option name */
  name?: string | null;
}

export interface EncryptedOptionDao {
  shortCode?: string | null;
  values?: EncryptedOptionValueDao[] | null;
}

export interface EncryptedOptionValueDao {
  cipher?: CipherTextDto;
  proofOfZeroOrOne?: ProofOfZeroOrOneDao;
}

/**
 * Represents a plaintext message
 */
export interface Message {
  /**
   * Numeric message
   * @format int32
   */
  m?: number;
}

/**
 * Contains the data needed for printing a ballot on paper.
 */
export interface PrintBallotDto {
  /** Ballot identifier */
  ballotId?: string | null;

  /** Selectable election options / candidates. */
  options?: PrintOptionDto[] | null;
}

/**
 * Represents one selectable option / candidate
 */
export interface PrintOptionDto {
  /** Name of the option / candidate */
  name?: string | null;

  /** First short code (from first virtual ballot) */
  shortCode1?: string | null;

  /** Second short code (from second virtual ballot) */
  shortCode2?: string | null;
}

export interface ProofOfContainingOneDao {
  u?: string | null;
  v?: string | null;
  s?: string | null;
}

export interface ProofOfZeroOrOneDao {
  u0?: string | null;
  u1?: string | null;
  v0?: string | null;
  v1?: string | null;
  c0?: string | null;
  c1?: string | null;
  r0?: string | null;
  r1?: string | null;
}

export interface Registration {
  name?: string | null;

  /** @format uri */
  endpoint?: string | null;
  account?: Account;
  enode?: string | null;
}

/**
 * Represents a registered consensus node.
 */
export interface RegistrationDto {
  /** Name of the consensus node. */
  name?: string | null;

  /**
   * API Endpoint for calling the consensus node.
   * @format uri
   */
  endpoint?: string | null;

  /** Public key of the consensus node. */
  publicKeys?: string[] | null;
}

export interface SumProofDao {
  cipher?: CipherTextDto;
  proofOfContainingOne?: ProofOfContainingOneDao;
}

export interface VirtualBallotDao {
  code?: string | null;
  rowProofs?: SumProofDao[] | null;
  columnProofs?: SumProofDao[] | null;
  encryptedOptions?: EncryptedOptionDao[] | null;
}

export type QueryParamsType = Record<string | number, any>;
export type ResponseFormat = keyof Omit<Body, "body" | "bodyUsed">;

export interface FullRequestParams extends Omit<RequestInit, "body"> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean;
  /** request path */
  path: string;
  /** content type of request body */
  type?: ContentType;
  /** query params */
  query?: QueryParamsType;
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseFormat;
  /** request body */
  body?: unknown;
  /** base url */
  baseUrl?: string;
  /** request cancellation token */
  cancelToken?: CancelToken;
}

export type RequestParams = Omit<FullRequestParams, "body" | "method" | "query" | "path">;

export interface ApiConfig<SecurityDataType = unknown> {
  baseUrl?: string;
  baseApiParams?: Omit<RequestParams, "baseUrl" | "cancelToken" | "signal">;
  securityWorker?: (securityData: SecurityDataType | null) => Promise<RequestParams | void> | RequestParams | void;
  customFetch?: typeof fetch;
}

export interface HttpResponse<D extends unknown, E extends unknown = unknown> extends Response {
  data: D;
  error: E;
}

type CancelToken = Symbol | string | number;

export enum ContentType {
  Json = "application/json",
  FormData = "multipart/form-data",
  UrlEncoded = "application/x-www-form-urlencoded",
}

export class HttpClient<SecurityDataType = unknown> {
  public baseUrl: string = "";
  private securityData: SecurityDataType | null = null;
  private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"];
  private abortControllers = new Map<CancelToken, AbortController>();
  private customFetch = (...fetchParams: Parameters<typeof fetch>) => fetch(...fetchParams);

  private baseApiParams: RequestParams = {
    credentials: "same-origin",
    headers: {},
    redirect: "follow",
    referrerPolicy: "no-referrer",
  };

  constructor(apiConfig: ApiConfig<SecurityDataType> = {}) {
    Object.assign(this, apiConfig);
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data;
  };

  private encodeQueryParam(key: string, value: any) {
    const encodedKey = encodeURIComponent(key);
    return `${encodedKey}=${encodeURIComponent(typeof value === "number" ? value : `${value}`)}`;
  }

  private addQueryParam(query: QueryParamsType, key: string) {
    return this.encodeQueryParam(key, query[key]);
  }

  private addArrayQueryParam(query: QueryParamsType, key: string) {
    const value = query[key];
    return value.map((v: any) => this.encodeQueryParam(key, v)).join("&");
  }

  protected toQueryString(rawQuery?: QueryParamsType): string {
    const query = rawQuery || {};
    const keys = Object.keys(query).filter((key) => "undefined" !== typeof query[key]);
    return keys
        .map((key) => (Array.isArray(query[key]) ? this.addArrayQueryParam(query, key) : this.addQueryParam(query, key)))
        .join("&");
  }

  protected addQueryParams(rawQuery?: QueryParamsType): string {
    const queryString = this.toQueryString(rawQuery);
    return queryString ? `?${queryString}` : "";
  }

  private contentFormatters: Record<ContentType, (input: any) => any> = {
    [ContentType.Json]: (input: any) =>
        input !== null && (typeof input === "object" || typeof input === "string") ? JSON.stringify(input) : input,
    [ContentType.FormData]: (input: any) =>
        Object.keys(input || {}).reduce((formData, key) => {
          const property = input[key];
          formData.append(
              key,
              property instanceof Blob
                  ? property
                  : typeof property === "object" && property !== null
                      ? JSON.stringify(property)
                      : `${property}`,
          );
          return formData;
        }, new FormData()),
    [ContentType.UrlEncoded]: (input: any) => this.toQueryString(input),
  };

  private mergeRequestParams(params1: RequestParams, params2?: RequestParams): RequestParams {
    return {
      ...this.baseApiParams,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...(this.baseApiParams.headers || {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      },
    };
  }

  private createAbortSignal = (cancelToken: CancelToken): AbortSignal | undefined => {
    if (this.abortControllers.has(cancelToken)) {
      const abortController = this.abortControllers.get(cancelToken);
      if (abortController) {
        return abortController.signal;
      }
      return void 0;
    }

    const abortController = new AbortController();
    this.abortControllers.set(cancelToken, abortController);
    return abortController.signal;
  };

  public abortRequest = (cancelToken: CancelToken) => {
    const abortController = this.abortControllers.get(cancelToken);

    if (abortController) {
      abortController.abort();
      this.abortControllers.delete(cancelToken);
    }
  };

  public request = async <T = any, E = any>({
                                              body,
                                              secure,
                                              path,
                                              type,
                                              query,
                                              format,
                                              baseUrl,
                                              cancelToken,
                                              ...params
                                            }: FullRequestParams): Promise<HttpResponse<T, E>> => {
    const secureParams =
        ((typeof secure === "boolean" ? secure : this.baseApiParams.secure) &&
            this.securityWorker &&
            (await this.securityWorker(this.securityData))) ||
        {};
    const requestParams = this.mergeRequestParams(params, secureParams);
    const queryString = query && this.toQueryString(query);
    const payloadFormatter = this.contentFormatters[type || ContentType.Json];
    const responseFormat = format || requestParams.format;

    return this.customFetch(`${baseUrl || this.baseUrl || ""}${path}${queryString ? `?${queryString}` : ""}`, {
      ...requestParams,
      headers: {
        ...(type && type !== ContentType.FormData ? { "Content-Type": type } : {}),
        ...(requestParams.headers || {}),
      },
      signal: cancelToken ? this.createAbortSignal(cancelToken) : void 0,
      body: typeof body === "undefined" || body === null ? null : payloadFormatter(body),
    }).then(async (response) => {
      const r = response as HttpResponse<T, E>;
      r.data = null as unknown as T;
      r.error = null as unknown as E;

      const data = !responseFormat
          ? r
          : await response[responseFormat]()
              .then((data) => {
                if (r.ok) {
                  r.data = data;
                } else {
                  r.error = data;
                }
                return r;
              })
              .catch((e) => {
                r.error = e;
                return r;
              });

      if (cancelToken) {
        this.abortControllers.delete(cancelToken);
      }

      if (!response.ok) throw data;
      return data;
    });
  };
}

/**
 * @title Voting Authority API
 * @version v1
 */
export class Api<SecurityDataType extends unknown> extends HttpClient<SecurityDataType> {
  api = {
    /**
     * No description
     *
     * @tags Ballots
     * @name ElectionsBallotsEncryptedDetail
     * @summary TODO: refactor, only temporary for demo purposes
     * @request GET:/api/elections/{electionId}/ballots/encrypted
     */
    electionsBallotsEncryptedDetail: (electionId: string, query?: { id?: string }, params: RequestParams = {}) =>
        this.request<VirtualBallotDao[], any>({
          path: `/api/elections/${electionId}/ballots/encrypted`,
          method: "GET",
          query: query,
          format: "json",
          ...params,
        }),

    /**
     * No description
     *
     * @tags Ballots
     * @name ElectionsBallotsDetail
     * @summary Shows the ballot data needed for printing a paper ballot.
     * @request GET:/api/elections/{electionId}/ballots
     */
    electionsBallotsDetail: (electionId: string, query?: { id?: string }, params: RequestParams = {}) =>
        this.request<PrintBallotDto, any>({
          path: `/api/elections/${electionId}/ballots`,
          method: "GET",
          query: query,
          format: "json",
          ...params,
        }),

    /**
     * No description
     *
     * @tags Ballots
     * @name ElectionsBallotsCreate
     * @summary Generates new ballots, stores the encryptions on IPFS, publishes the evidence and the IPFS CIDs on the smart contract,
     and persists the plaintext print ballots onto the database.
     * @request POST:/api/elections/{electionId}/ballots
     */
    electionsBallotsCreate: (electionId: string, data: BallotGenerationDto, params: RequestParams = {}) =>
        this.request<void, any>({
          path: `/api/elections/${electionId}/ballots`,
          method: "POST",
          body: data,
          type: ContentType.Json,
          ...params,
        }),

    /**
     * No description
     *
     * @tags Ballots
     * @name ElectionsBallotsPdfDetail
     * @summary Generates a PDF for the specified ballot.
     * @request GET:/api/elections/{electionId}/ballots/pdf
     */
    electionsBallotsPdfDetail: (electionId: string, query?: { ballotId?: string }, params: RequestParams = {}) =>
        this.request<void, any>({
          path: `/api/elections/${electionId}/ballots/pdf`,
          method: "GET",
          query: query,
          ...params,
        }),

    /**
     * No description
     *
     * @tags Ballots
     * @name ElectionsBallotsPdfAllDetail
     * @request GET:/api/elections/{electionId}/ballots/pdf/all
     */
    electionsBallotsPdfAllDetail: (
        electionId: string,
        query?: { numberOfBallots?: number },
        params: RequestParams = {},
    ) =>
        this.request<void, any>({
          path: `/api/elections/${electionId}/ballots/pdf/all`,
          method: "GET",
          query: query,
          ...params,
        }),

    /**
     * No description
     *
     * @tags Blockchain
     * @name BlockchainCreate
     * @summary Initializes the Proof-of-Authority blockchain using the consensus nodes registered.
     * @request POST:/api/blockchain
     */
    blockchainCreate: (data: BlockchainDto, params: RequestParams = {}) =>
        this.request<BlockchainDto, any>({
          path: `/api/blockchain`,
          method: "POST",
          body: data,
          type: ContentType.Json,
          format: "json",
          ...params,
        }),

    /**
     * No description
     *
     * @tags Blockchain
     * @name BlockchainList
     * @summary Returns the blockchain instance. Exists only once.
     * @request GET:/api/blockchain
     */
    blockchainList: (params: RequestParams = {}) =>
        this.request<BlockchainDto, any>({
          path: `/api/blockchain`,
          method: "GET",
          format: "json",
          ...params,
        }),

    /**
     * No description
     *
     * @tags Elections
     * @name ElectionsCreate
     * @summary Create a new election.
     * @request POST:/api/elections
     */
    electionsCreate: (data: ElectionDto, params: RequestParams = {}) =>
        this.request<ElectionDto, any>({
          path: `/api/elections`,
          method: "POST",
          body: data,
          type: ContentType.Json,
          format: "json",
          ...params,
        }),

    /**
     * No description
     *
     * @tags Elections
     * @name ElectionsList
     * @summary Provides a list of all elections.
     * @request GET:/api/elections
     */
    electionsList: (params: RequestParams = {}) =>
        this.request<ElectionDto[], any>({
          path: `/api/elections`,
          method: "GET",
          format: "json",
          ...params,
        }),

    /**
     * No description
     *
     * @tags Elections
     * @name ElectionsDetail
     * @summary Provides the election with the specified id.
     * @request GET:/api/elections/{id}
     */
    electionsDetail: (id: string, params: RequestParams = {}) =>
        this.request<ElectionDto, any>({
          path: `/api/elections/${id}`,
          method: "GET",
          format: "json",
          ...params,
        }),

    /**
     * No description
     *
     * @tags Elections
     * @name ElectionsUpdate
     * @summary Updates a specific election.
     * @request PUT:/api/elections/{id}
     */
    electionsUpdate: (id: string, data: ElectionDto, params: RequestParams = {}) =>
        this.request<Election, any>({
          path: `/api/elections/${id}`,
          method: "PUT",
          body: data,
          type: ContentType.Json,
          format: "json",
          ...params,
        }),

    /**
     * No description
     *
     * @tags Elections
     * @name ElectionsDelete
     * @summary Removes an election.
     * @request DELETE:/api/elections/{id}
     */
    electionsDelete: (id: string, params: RequestParams = {}) =>
        this.request<void, any>({
          path: `/api/elections/${id}`,
          method: "DELETE",
          ...params,
        }),

    /**
     * No description
     *
     * @tags Elections
     * @name ElectionsPublicKeyUpdate
     * @summary Combines and stores the public keys of all registered consensus nodes of the specified election.
     * @request PUT:/api/elections/{id}/public-key
     */
    electionsPublicKeyUpdate: (id: string, params: RequestParams = {}) =>
        this.request<ElectionDto, any>({
          path: `/api/elections/${id}/public-key`,
          method: "PUT",
          format: "json",
          ...params,
        }),

    /**
     * No description
     *
     * @tags Elections
     * @name ElectionsContractCreate
     * @request POST:/api/elections/{id}/contract
     */
    electionsContractCreate: (id: string, params: RequestParams = {}) =>
        this.request<ElectionDto, any>({
          path: `/api/elections/${id}/contract`,
          method: "POST",
          format: "json",
          ...params,
        }),

    /**
     * No description
     *
     * @tags Elections
     * @name ElectionsEncryptCreate
     * @summary For testing purposes
     * @request POST:/api/elections/{id}/encrypt
     */
    electionsEncryptCreate: (id: string, data: Message, params: RequestParams = {}) =>
        this.request<Cipher, any>({
          path: `/api/elections/${id}/encrypt`,
          method: "POST",
          body: data,
          type: ContentType.Json,
          format: "json",
          ...params,
        }),

    /**
     * No description
     *
     * @tags Elections
     * @name ElectionsDecryptCreate
     * @summary For testing purposes
     * @request POST:/api/elections/{id}/decrypt
     */
    electionsDecryptCreate: (id: string, data: Cipher, params: RequestParams = {}) =>
        this.request<Message, any>({
          path: `/api/elections/${id}/decrypt`,
          method: "POST",
          body: data,
          type: ContentType.Json,
          format: "json",
          ...params,
        }),
  };
}
